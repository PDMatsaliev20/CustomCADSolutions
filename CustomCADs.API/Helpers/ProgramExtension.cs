using CustomCADs.API.Mappers;
using CustomCADs.Application;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Email;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using CustomCADs.Infrastructure.Identity.Managers;
using CustomCADs.Infrastructure.Payment;
using CustomCADs.Persistence;
using CustomCADs.Persistence.Repositories;
using CustomCADs.Persistence.Repositories.Cads;
using CustomCADs.Persistence.Repositories.Categories;
using CustomCADs.Persistence.Repositories.Orders;
using CustomCADs.Persistence.Repositories.Roles;
using CustomCADs.Persistence.Repositories.Users;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;
using System.Text.Json;
using static CustomCADs.Domain.DataConstants.RoleConstants;
using static Microsoft.AspNetCore.Http.StatusCodes;
using Order = CustomCADs.Domain.Entities.Order;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection
#pragma warning restore IDE0130
{
    public static class ProgramExtension
    {
        public static void AddApplicationContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("RealConnection")
                    ?? throw new KeyNotFoundException("Could not find connection string 'RealConnection'.");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDbTracker, DbTracker>();

            services.AddScoped<IOrderQueries, OrderQueries>();
            services.AddScoped<ICadQueries, CadQueries>();
            services.AddScoped<ICategoryQueries, CategoryQueries>();
            services.AddScoped<IUserQueries, UserQueries>();
            services.AddScoped<IRoleQueries, RoleQueries>();

            services.AddScoped<ICommands<Order>, OrderCommands>();
            services.AddScoped<ICommands<Cad>, CadCommands>();
            services.AddScoped<ICommands<Category>, CategoryCommands>();
            services.AddScoped<ICommands<User>, UserCommands>();
            services.AddScoped<ICommands<Role>, RoleCommands>();
        }

        public static void AddIdentityContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("IdentityConnection")
                    ?? throw new KeyNotFoundException("Could not find connection string 'IdentityConnection'.");
            services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));

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

            services.AddScoped<IAppUserManager, AppUserManager>();
            services.AddScoped<IAppRoleManager, AppRoleManager>();
        }

        public static IServiceCollection AddStripe(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<StripeKeys>(config.GetSection("Stripe"));
            services.AddScoped<PaymentIntentService>();
            services.AddScoped<IPaymentService, StripeService>();
            return services;
        }

        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICadService, CadService>();
            services.AddScoped<IDesignerService, DesignerService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IEmailService, MailKitService>();
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            CadsMapper.Map();
            OrdersMapper.Map();
            UsersMapper.Map();
            return services.AddAutoMapper(typeof(TestsErrorMessages));
        }

        public static IMvcBuilder AddEndpoints(this IServiceCollection services)
        {
            return services
                .AddFastEndpoints()
                .AddControllers();
        }

        public static void AddJsonAndXml(this IMvcBuilder mvc)
        {
            mvc.AddNewtonsoftJson();
            mvc.AddXmlDataContractSerializerFormatters();
        }

        public static IWebHostBuilder AddUploadSizeLimitations(this IWebHostBuilder webhost, int limit = 300_000_000)
        {
            return webhost.ConfigureKestrel(o => o.Limits.MaxRequestBodySize = limit);
        }

        public static void AddApiConfigurations(this IServiceCollection services)
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

        public static IServiceCollection AddAuthWithCookie(this IServiceCollection services, IConfiguration config)
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

            return services;
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

        public static IApplicationBuilder UseStaticFilesAndCads(this IApplicationBuilder app)
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
                }
            });

            return app;
        }

        public static void UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                try
                {
                    await next.Invoke().ConfigureAwait(false);
                }
                catch (KeyNotFoundException ex)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = Status404NotFound;
                    string result = JsonSerializer.Serialize(new { error = "Resource Not Found", message = ex.Message });
                    await context.Response.WriteAsync(result).ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = Status409Conflict;
                    string result = JsonSerializer.Serialize(new { error = "Database Conflict Ocurred", message = ex.Message });
                    await context.Response.WriteAsync(result).ConfigureAwait(false);
                }
                catch (DbUpdateException ex)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = Status400BadRequest;
                    string result = JsonSerializer.Serialize(new { error = "Database Error", message = ex.Message });
                    await context.Response.WriteAsync(result).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = Status500InternalServerError;
                    string result = JsonSerializer.Serialize(new { error = "Internal Server Error", message = ex.Message });
                    await context.Response.WriteAsync(result).ConfigureAwait(false);
                }
            });
        }

        public static async Task UseRolesAsync(this IServiceProvider service, string[] roles)
        {
            using IServiceScope scope = service.CreateScope();
            var appRoleManager = scope.ServiceProvider.GetRequiredService<IAppRoleManager>();
            var roleService = scope.ServiceProvider.GetRequiredService<IRoleService>();

            foreach (string role in roles)
            {
                if (!await appRoleManager.RoleExistsAsync(role).ConfigureAwait(false))
                {
                    await appRoleManager.CreateAsync(new AppRole(role)).ConfigureAwait(false);
                }

                if (!await roleService.ExistsByNameAsync(role).ConfigureAwait(false))
                {
                    string? description = role switch
                    {
                        Client => ClientDescription,
                        Contributor => ContributorDescription,
                        Designer => DesignerDescription,
                        Admin => AdminDescription,
                        _ => "Description missing."
                    };
                    await roleService.CreateAsync(new RoleModel()
                    {
                        Name = role,
                        Description = description
                    }).ConfigureAwait(false);
                }
            }
        }

        public static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
        {
            return app
                .UseFastEndpoints(cfg => cfg.Endpoints.RoutePrefix = "API")
                .UseSwaggerGen();
        }
    }
}
