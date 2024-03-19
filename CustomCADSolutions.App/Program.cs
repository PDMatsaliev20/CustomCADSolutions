var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddControllersWithViews().AddViewLocalizer();

System.Globalization.CultureInfo[] cultures = { new("en-US"), new("bg-BG") };
builder.Services.AddLocalizer(cultures);

string[] roles = { "Administrator", "Designer", "Contributer", "Client" };
builder.Services.AddRoles(roles);

builder.Services.AddAbstractions();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Home/Unauthorized";
});

var app = builder.Build();

app.UseLocalizion("en-US", cultures);

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}
    app.UseExceptionHandler("/Home/StatusCodeHandler");
app.UseStatusCodePagesWithReExecute("/Home/StatusCodeHandler", "?statusCode={0}");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

await app.Services.UseRolesAsync(roles);

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

await app.RunAsync();