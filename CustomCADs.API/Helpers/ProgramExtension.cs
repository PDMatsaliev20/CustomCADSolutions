using CustomCADs.Application.Contracts;
using CustomCADs.Application.Mappings;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Services;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities.Identity;
using CustomCADs.Infrastructure.Data;
using CustomCADs.Infrastructure.Payment;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProgramExtension
    {
        public static void AddCadContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("RealConnection")
                    ?? throw new KeyNotFoundException("Could not find connection string 'RealConnection'.");
            services.AddDbContext<CadContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IRepository, Repository>();
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
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICadService, CadService>();
            services.AddScoped<ICategoryService, CategoryService>();
        }

        public static IServiceCollection AddApplicationAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(CategoryProfile), typeof(CadProfile), typeof(OrderProfile));
            return services;
        }

        public static void AddJsonAndXml(this IMvcBuilder mvc)
        {
            mvc.AddNewtonsoftJson();
            mvc.AddXmlDataContractSerializerFormatters();
        }

        public static void AddApiConfigurations(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
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
                    await roleManager.CreateAsync(new AppRole(role));
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
                AppUser user = new()
                {
                    UserName = username,
                    Email = email,
                };

                var result = await userManager.CreateAsync(user, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role);
                }
            }
        }
    }
}
