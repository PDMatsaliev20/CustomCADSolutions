using CustomCADs.API.Identity;
using CustomCADs.Application;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Payment;
using CustomCADs.Persistence;
using CustomCADs.Persistence.Repositories;
using CustomCADs.Persistence.Repositories.Cads;
using CustomCADs.Persistence.Repositories.Categories;
using CustomCADs.Persistence.Repositories.Orders;
using CustomCADs.Persistence.Repositories.Roles;
using CustomCADs.Persistence.Repositories.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Stripe;
using System.Text;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProgramExtension
    {
        public static void AddApplicationContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("RealConnection")
                    ?? throw new KeyNotFoundException("Could not find connection string 'RealConnection'.");
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDbTracker, DbTracker>();

            services.AddScoped<IQueries<Order, int>, OrderQueries>();
            services.AddScoped<IQueries<Cad, int>, CadQueries>();
            services.AddScoped<IQueries<Category, int>, CategoryQueries>();
            services.AddScoped<IQueries<User, string>, UserQueries>();
            services.AddScoped<IQueries<Role, string>, RoleQueries>();

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
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddEntityFrameworkStores<IdentityContext>();

            services.AddScoped<AppUserManager>();
            services.AddScoped<AppSignInManager>();
            services.AddScoped<AppRoleManager>();
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
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(typeof(Program))
                .AddAutoMapper(typeof(TestsErrorMessages));
        }

        public static void AddJsonAndXml(this IMvcBuilder mvc)
        {
            mvc.AddNewtonsoftJson();
            mvc.AddXmlDataContractSerializerFormatters();
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
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "SwaggerAnnotation.xml"));
            });
        }

        public static void AddCorsForReact(this IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("https://localhost:5173")
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

        public static async Task UseRolesAsync(this IServiceProvider service, string[] roles)
        {
            using IServiceScope scope = service.CreateScope();
            var appRoleManager = scope.ServiceProvider.GetRequiredService<AppRoleManager>();
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

        public static async Task UseCategoriesAsync(this IServiceProvider service)
        {
            using IServiceScope scope = service.CreateScope();
            var categoryService = scope.ServiceProvider.GetRequiredService<ICategoryService>();

            IEnumerable<CategoryModel> existingCategoreis = await categoryService.GetAllAsync().ConfigureAwait(false);
            foreach (string category in new string[] { "Animals", "Characters", "Electronics", "Fashion", "Furniture", "Nature", "Science", "Sports", "Toys", "Vehicles", "Others" })
            {
                if (!existingCategoreis.Select(c => c.Name).Contains(category))
                {
                    await categoryService.CreateAsync(new CategoryModel() { Name = category });
                }
            }
        }

        public static async Task UseAppUsers(this IServiceProvider service, IConfiguration config, Dictionary<string, string> users)
        {
            using IServiceScope scope = service.CreateScope();
            var appUserManager = scope.ServiceProvider.GetRequiredService<AppUserManager>();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();

            int i = 1;
            foreach (KeyValuePair<string, string> user in users)
            {
                string role = user.Key;
                string username = user.Value;
                string email = $"user{i++}@mail.com";
                string? password = config[$"Passwords:{role}"];
                if (password != null)
                {
                    await appUserManager.AddUserAsync(username, email, password, role);

                    if (!await userService.ExistsByNameAsync(username).ConfigureAwait(false))
                    {
                        await userService.CreateAsync(new() { UserName = username, Email = email, RoleName = role });
                    }
                }
            }
        }

        private static async Task AddUserAsync(this AppUserManager appUserManager, string username, string email, string password, string role)
        {
            if (await appUserManager.FindByEmailAsync(email) == null)
            {
                AppUser user = new(username, email);

                var result = await appUserManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await appUserManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
