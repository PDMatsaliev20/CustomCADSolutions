using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseLocalizion(this IApplicationBuilder app, string defaultCulture, params CultureInfo[] supportedCultures)
        {
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(defaultCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            return app;
        }

        public static async Task<IServiceProvider> UseRolesAsync(this IServiceProvider service, string[] roles)
        {
            using IServiceScope scope = service.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            foreach (string role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            return service;
        }

        public static IApplicationBuilder MapRoutes(this WebApplication app)
        {
            app.MapAreaControllerRoute(
                name: "AdminArea",
                areaName: "Admin",
                pattern: "Admin/{controller=Users}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "DesignerArea",
                areaName: "Designer",
                pattern: "Designer/{controller}/{action=All}/{id?}");

            app.MapAreaControllerRoute(
                name: "ContributerArea",
                areaName: "Contributer",
                pattern: "Contributer/{controller=Home}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "ClientArea",
                areaName: "Client",
                pattern: "Client/{controller=Home}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "IdentityArea",
                areaName: "Identity",
                pattern: "Identity/{controller=Account}/{action=Register}");

            app.MapDefaultControllerRoute();

            return app;
        }
    }
}
