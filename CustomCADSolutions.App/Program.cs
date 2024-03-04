using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Services;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using CustomCADSolutions.App.Resources.Shared;

var builder = WebApplication.CreateBuilder(args);

// Add database to the container.
var connectionString = builder.Configuration.GetConnectionString("RealConnection");
builder.Services.AddDbContext<CADContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Add identity to the container
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<CADContext>();

// Add services to the container
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
   .AddDataAnnotationsLocalization(options =>
   {
       options.DataAnnotationLocalizerProvider = (type, factory) =>
           factory.Create(typeof(SharedResources));
   });

// Add localization to the container
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en-US");
    
    var cultures = new CultureInfo[]
    {
        new("en-US"),
        new("bg-BG")
    };
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});

// Add roles to the container
string[] roles = { "Administrator", "Designer", "Contributer", "Client"};
builder.Services.AddAuthorization(options =>
{
    foreach (string role in roles)
    {
        options.AddPolicy(role, policy => policy.RequireRole(role));
    }
});

// Add abstraction levels to the container
builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<IConverter, Converter>();

builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICadService, CadService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

// Add redirection to the container
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

//  Build app
var app = builder.Build();

// Request Localization
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = new[] { new CultureInfo("bg-BG"), new CultureInfo("en-US") },
    SupportedUICultures = new[] { new CultureInfo("bg-BG"), new CultureInfo("en-US") }
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
