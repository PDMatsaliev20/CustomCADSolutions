using CustomCADs.API;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.UseCases.Roles.Commands.Create;
using CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using CustomCADs.Infrastructure.Email;
using CustomCADs.Infrastructure.Payment;
using FastEndpoints;
using FastEndpoints.Swagger;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static CustomCADs.Domain.Roles.RoleConstants;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection;

public static class ProgramExtension
{
    public static void AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        services.AddApplicationContext(config);
        services.AddQueries();
        services.AddCommands();
    }
    public static void AddIdentity(this IServiceCollection services, IConfiguration config)
    {
        services.AddIdentityContext(config);
        services.AddIdentityAppManagers();

        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.SignIn.RequireConfirmedEmail = true;
            options.SignIn.RequireConfirmedAccount = false;
            options.Password.RequireDigit = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        })
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();
    }

    public static void AddStripe(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<StripeKeys>(config.GetSection("Stripe"));
        services.AddStripeServices();
    }

    public static void AddEmail(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<EmailOptions>(config.GetSection("Email"));
        services.AddEmailServices();
    }

    public static void AddMappings(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ApiReference).Assembly);
    }

    public static void AddEndpoints(this IServiceCollection services)
    {
        services.AddFastEndpoints();
    }

    public static void AddUploadSizeLimitations(this IWebHostBuilder webhost, int limit = 300_000_000)
    {
        webhost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = limit);
    }

    public static void AddApiDocumentation(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.EnableAnnotations();
            c.SwaggerDoc("v1", new()
            {
                Title = "CustomCADs API",
                Description = "An API to Order, Purchase, Upload and Validate 3D Models",
                Contact = new() { Name = "Ivan", Email = "ivanangelov414@gmail.com" },
                License = new() { Name = "Apache License 2.0", Url = new Uri("https://www.apache.org/licenses/LICENSE-2.0") }
            });
        });
    }

    public static void AddCorsForReact(this IServiceCollection services, IConfiguration config)
    {
        string clientUrl = config["URLs:Client"] ?? throw new ArgumentNullException("No Client URL provided.");
        services.AddCors(opt =>
        {
            opt.AddDefaultPolicy(builder =>
            {
                builder.WithOrigins(clientUrl)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
            });
        });
    }

    public static void AddAuthAndJwt(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(opt =>
        {
            string? secretKey = config["JwtSettings:SecretKey"];
            ArgumentNullException.ThrowIfNull(secretKey, nameof(secretKey));

            opt.TokenValidationParameters = new()
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config["JwtSettings:Issuer"],
                ValidAudience = config["JwtSettings:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };

            opt.Events = new()
            {
                OnMessageReceived = context =>
                {
                    context.Token = context.Request.Cookies["jwt"];
                    return Task.CompletedTask;
                },
            };
        });
    }

    public static void AddRoles(this IServiceCollection services, IEnumerable<string> roles)
    {
        services.AddAuthorization(options =>
        {
            foreach (string role in roles)
            {
                options.AddPolicy(role, policy => policy.RequireRole(role));
            }
        });
    }

    public static void UseStaticFilesAndCads(this IApplicationBuilder app)
    {
        app.UseStaticFiles(new StaticFileOptions()
        {
            ContentTypeProvider = new FileExtensionContentTypeProvider()
            {
                Mappings =
                {
                    [".glb"] = "model/gltf-binary",
                    [".gltf"] = "model/gltf+json"
                }
            },
            OnPrepareResponse = sfrc =>
            {
                sfrc.Context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
                sfrc.Context.Response.Headers.Append("Access-Control-Allow-Methods", "GET, OPTIONS");
                sfrc.Context.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type");
            }
        });
    }

    public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(errorApp =>
        {
            errorApp.Run(async context =>
            {
                var ehf = context.Features.Get<IExceptionHandlerFeature>();
                var ex = ehf?.Error;

                if (ex != null)
                {
                    var handler = new GlobalExceptionHandler();
                    await handler.TryHandleAsync(context, ex, context.RequestAborted).ConfigureAwait(false);
                }
            });
        });
    }

    public static async Task UseRolesAsync(this IServiceProvider service, string[] roles)
    {
        using IServiceScope scope = service.CreateScope();
        var appRoleManager = scope.ServiceProvider.GetRequiredService<IAppRoleManager>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        foreach (string role in roles)
        {
            if (!await appRoleManager.RoleExistsAsync(role).ConfigureAwait(false))
            {
                await appRoleManager.CreateAsync(new AppRole(role)).ConfigureAwait(false);
            }

            RoleExistsByNameQuery query = new(role);
            if (!await mediator.Send(query).ConfigureAwait(false))
            {
                string? description = role switch
                {
                    Client => ClientDescription,
                    Contributor => ContributorDescription,
                    Designer => DesignerDescription,
                    Admin => AdminDescription,
                    _ => "Description missing."
                };

                RoleModel model = new()
                {
                    Name = role,
                    Description = description
                };
                CreateRoleCommand command = new(model);
                await mediator.Send(command).ConfigureAwait(false);
            }
        }
    }

    public static void UseEndpoints(this IApplicationBuilder app)
    {
        app.UseFastEndpoints(cfg => cfg.Endpoints.RoutePrefix = "API").UseSwaggerGen();
    }
}
