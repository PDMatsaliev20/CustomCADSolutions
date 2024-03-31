WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Database, Identity, MVC, API and CORS
builder.Services.AddApplicationDbContext(builder.Configuration);
builder.Services.AddApplicationIdentity();
builder.Services.AddInbuiltServices();
builder.Services.AddAPI();

// Localizer
System.Globalization.CultureInfo[] cultures = { new("en-US"), new("bg-BG") };
builder.Services.AddLocalizer(cultures);

// Roles, Stripe and Swagger
string[] roles = { "Administrator", "Designer", "Contributer", "Client" };
builder.Services.AddRoles(roles);
builder.Services.AddStripe(builder.Configuration);
builder.Services.AddSwaggerGen();

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
else if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

await app.Services.UseRolesAsync(roles);
app.MapRoutes();

await app.RunAsync();