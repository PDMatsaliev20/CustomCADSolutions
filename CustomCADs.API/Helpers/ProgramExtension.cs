﻿using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
using CustomCADs.Core.Services;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities.Identity;
using CustomCADs.Infrastructure.Data;
using CustomCADs.Infrastructure.Payment;
using CustomCADs.Infrastructure.Payment.Contracts;
using CustomCADs.Infrastructure.Payment.Services;
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
            services.AddScoped<IStripeService, StripeService>();
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
            })
                .AddCookie(options =>
                {
                    options.LoginPath = "/API/Identity/Login";
                    options.LogoutPath = "/API/Identity/Logout";
                    options.AccessDeniedPath = "/AccessDenied";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.None;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.SlidingExpiration = true;
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
                string password = config[$"Passwords:{role}"]
                    ?? throw new KeyNotFoundException($"No password found for {role}");

                await userManager.AddUserAsync(username, email, password, role);
            }
        }

        private static async Task AddUserAsync(this UserManager<AppUser> userManager, string username, string email, string password, string role)
        {
            if (await userManager.FindByNameAsync(username) == null)
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
}
