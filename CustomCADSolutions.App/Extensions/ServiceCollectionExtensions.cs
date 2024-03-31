using CustomCADSolutions.App.Resources.Shared;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Services;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            string connectionString = config.GetConnectionString("RealConnection");
            services.AddDbContext<CADContext>(options => options.UseSqlServer(connectionString));
            services.AddScoped<IRepository, Repository>();
            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<CADContext>();

            return services;
        }

        public static IMvcBuilder AddInbuiltServices(this IServiceCollection services)
        {
            AutoValidateAntiforgeryTokenAttribute antiForgeryToken = new();
            var extensionFormat = LanguageViewLocationExpanderFormat.Suffix;

            return services
                .AddControllersWithViews(opt => opt.Filters.Add(antiForgeryToken))
                .AddViewLocalizer(extensionFormat, typeof(SharedResources));
        }

        private static IMvcBuilder AddViewLocalizer(this IMvcBuilder builder, LanguageViewLocationExpanderFormat format, Type resource)
        {
            builder.AddViewLocalization(format);
            builder.AddDataAnnotationsLocalization(options =>
                options.DataAnnotationLocalizerProvider = (type, factory) 
                    => factory.Create(resource));

            return builder;
        }

        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }

        public static IServiceCollection AddLocalizer(this IServiceCollection services, CultureInfo[] cultures)
        {
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new RequestCulture("en-US");

                options.SupportedCultures = cultures;
                options.SupportedUICultures = cultures;
            });

            return services;
        }

        public static IServiceCollection AddRoles(this IServiceCollection services, string[] roles)
        {
            services.AddAuthorization(options =>
            {
                foreach (string role in roles)
                {
                    options.AddPolicy(role, policy => policy.RequireRole(role));
                }
            });

            return services;
        }

        public static IServiceCollection AddAbstractions(this IServiceCollection services)
        {
            services.AddScoped<IConverter, Converter>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICadService, CadService>();
            services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }

        public static IServiceCollection AddStripe(this IServiceCollection services, IConfiguration config)
        {
            IConfigurationSection stripeSection = config.GetSection("Stripe");
            services.Configure<StripeSettings>(stripeSection);
            return services;
        }
    }

    public class StripeSettings
    {
        public string SecretKey { get; set; } = null!;
        public string PublishableKey { get; set; } = null!;
    }
}
