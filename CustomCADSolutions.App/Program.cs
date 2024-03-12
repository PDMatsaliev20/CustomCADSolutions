using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using CustomCADSolutions.App.Resources.Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity();

builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
            factory.Create(typeof(SharedResources));
    });

builder.Services.AddApplicationServices();

builder.Services.AddLocalizater();

string[] roles = { "Administrator", "Designer", "Contributer", "Client" };
builder.Services.AddRoles(roles);

builder.Services.AddAbstractions();

builder.Services.ConfigureApplicationCookie(options => options.LoginPath = "/Identity/Account/Login");

var app = builder.Build();

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new[] { new CultureInfo("bg-BG"), new CultureInfo("en-US") },
    SupportedUICultures = new[] { new CultureInfo("bg-BG"), new CultureInfo("en-US") }
});

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
 
app.UseAuthentication();
app.UseAuthorization();

using (IServiceScope scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    foreach (string role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}


app.MapAreaControllerRoute(
    name: "AdminArea",
    areaName: "Admin",
    pattern: "Admin/{controller=User}/{action=Index}/{id?}");

app.MapAreaControllerRoute(
    name: "DesignerArea",
    areaName: "Designer",
    pattern: "Designer/{controller=Home}/{action=Categories}/{id?}");

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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();