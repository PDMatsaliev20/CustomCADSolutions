WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Database, Identity, MVC, API and CORS
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddInbuiltServices();

// Localizer
System.Globalization.CultureInfo[] cultures = { new("en-US"), new("bg-BG") };
builder.Services.AddLocalizer(cultures);

// Roles, Stripe and Swagger
string[] roles = { "Administrator", "Designer", "Contributor", "Client" };
builder.Services.AddRoles(roles);
builder.Services.AddStripe(builder.Configuration);

// Abstractions and App Cookie
builder.Services.AddAbstractions();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Home/Unauthorized";
});

WebApplication app = builder.Build();

app.UseLocalizion("en-US", cultures);

if (app.Environment.IsProduction())
{
    app.UseProductionMiddlewares();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
await app.Services.UseRolesAsync(roles);

Dictionary<string, string> users = new()
{
    ["Administrator"] = "NinjataBG",
    ["Designer"] = "Designer",
    ["Contributor"] = "Contributor",
    ["Client"] = "Client",
};
await app.Services.UseAppUsers(app.Configuration, users);

app.MapRoutes();
await app.RunAsync();