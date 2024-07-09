using CustomCADs.App.Extensions;
using CustomCADs.App.Hubs;
using CustomCADs.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.StaticFiles;
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

        public static IApplicationBuilder UseAnimalErrors(this IApplicationBuilder app, string exceptionPath)
        {
            app.UseExceptionHandler(exceptionPath);
            app.UseStatusCodePagesWithReExecute(exceptionPath, "?statusCode={0}");

            return app;
        }

        public static IApplicationBuilder UseStaticFilesWithGltf(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions()
            {
                ContentTypeProvider = new FileExtensionContentTypeProvider()
                {
                    Mappings =
                    {
                        [".gltf"] = "model/gltf+json",
                        [".glb"] = "model/gltf-binary"
                    }
                }
            });

            return app;
        }

        public static async Task<IServiceProvider> UseRolesAsync(this IServiceProvider service, string[] roles)
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

            return service;
        }

        public static async Task<IServiceProvider> UseAppUsers(this IServiceProvider service, IConfiguration config, Dictionary<string, string> users)
        {
            using IServiceScope scope = service.CreateScope();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            int i = 1;
            foreach (KeyValuePair<string, string> user in users)
            {
                string role = user.Key;
                string username = user.Value;
                string email = $"user{i++}@mail.com";
                string password = config[$"Passwords:{role}"];
                await userManager.AddUserAsync(username, email, password, role);
            }

            return service;
        }

        public static IApplicationBuilder MapRoutes(this WebApplication app)
        {
            var defaults = new { Controller = "Home", Action = "Index" };

            app.MapAreaControllerRoute(
                name: "AdminArea",
                areaName: "Admin",
                pattern: "Admin/{controller}/{action}/{id?}",
                defaults: defaults);

            app.MapAreaControllerRoute(
                name: "DesignerArea",
                areaName: "Designer",
                pattern: "Designer/{controller}/{action}/{id?}",
                defaults: defaults);

            app.MapDefaultControllerRoute();
            app.MapHub<CadsHub>("/cadsHub");
            
            return app;
        }
    }
}
