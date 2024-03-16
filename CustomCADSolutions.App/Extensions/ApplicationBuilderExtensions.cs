using Microsoft.AspNetCore.Authentication;
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
    }
}
