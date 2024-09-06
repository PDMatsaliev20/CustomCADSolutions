using CustomCADs.Application.Contracts;
using CustomCADs.Application.Mappings;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data.Identity;
using CustomCADs.Infrastructure.Data;
using CustomCADs.Infrastructure.Data.Repositories;
using CustomCADs.Infrastructure.Data.Repositories.Command;
using CustomCADs.Infrastructure.Data.Repositories.Query;
using CustomCADs.Infrastructure.Payment;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Stripe;
using static CustomCADs.Domain.DataConstants.RoleConstants;
using CustomCADs.Application;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProgramExtension
    {
        public static void AddCadContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("RealConnection")
                    ?? throw new KeyNotFoundException("Could not find connection string 'RealConnection'.");
            services.AddDbContext<CadContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IDbTracker, DbTracker>();            

            services.AddScoped<IQueryRepository<Order, int>, OrderQueryRepository>();
            services.AddScoped<IQueryRepository<Cad, int>, CadQueryRepository>();
            services.AddScoped<IQueryRepository<Category, int>, CategoryQueryRepository>();

            services.AddScoped<ICommandRepository<Order>, OrderCommandRepository>();
            services.AddScoped<ICommandRepository<Cad>, CadCommandRepository>();
            services.AddScoped<ICommandRepository<Category>, CategoryCommandRepository>();
        }

        public static void AddAppIdentity(this IServiceCollection services)
        {
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
            .AddEntityFrameworkStores<CadContext>()
            .AddDefaultTokenProviders();
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
        }

        public static IServiceCollection AddMappings(this IServiceCollection services)
        {
            return services
                .AddAutoMapper(typeof(Program))
                .AddAutoMapper(typeof(TestsErrorMessages))
                .AddAutoMapper(typeof(Mappings));
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
                    License = new() { Name = "Apache License 2.0", Url= new Uri("https://www.apache.org/licenses/LICENSE-2.0") }
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

        public static IServiceCollection AddAuthWithCookie(this IServiceCollection services)
        {
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultForbidScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
                    options.Events = new()
                    {
                        OnRedirectToLogin = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            return Task.CompletedTask;
                        },
                        OnRedirectToAccessDenied = context =>
                        {
                            context.Response.StatusCode = StatusCodes.Status403Forbidden;
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
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();
            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    string? description = role switch
                    {
                        Client => ClientDescription,
                        Contributor => ContributorDescription,
                        Designer => DesignerDescription,
                        Admin => AdminDescription,
                        _ => null
                    };
                    await roleManager.CreateAsync(new AppRole(role, description));
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
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            int i = 1;
            foreach (KeyValuePair<string, string> user in users)
            {
                string role = user.Key;
                string username = user.Value;
                string email = $"user{i++}@mail.com";
                string? password = config[$"Passwords:{role}"];
                if (password != null)
                {
                    await userManager.AddUserAsync(username, email, password, role);
                }
            }
        }

        private static async Task AddUserAsync(this UserManager<AppUser> userManager, string username, string email, string password, string role)
        {
            if (await userManager.FindByEmailAsync(email) == null)
            {
                AppUser user = new(username, email);

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
