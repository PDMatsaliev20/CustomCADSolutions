WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Db, Identity, Mvc and ViewLocalizer
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddControllersWithViews().AddViewLocalizer();

// Localizer
System.Globalization.CultureInfo[] cultures = { new("en-US"), new("bg-BG") };
builder.Services.AddLocalizer(cultures);

// Roles
string[] roles = { "Administrator", "Designer", "Contributer", "Client" };
builder.Services.AddRoles(roles);

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
    app.UseHsts();
    app.UseExceptionHandler("/Home/StatusCodeHandler");
    app.UseStatusCodePagesWithReExecute("/Home/StatusCodeHandler", "?statusCode={0}");
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

await app.Services.UseRolesAsync(roles);
app.MapRoutes();

await app.RunAsync();