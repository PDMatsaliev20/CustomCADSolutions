using static CustomCADs.Domain.DataConstants.RoleConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCadContext(builder.Configuration);
builder.Services.AddAppIdentity();
builder.Services.AddStripe(builder.Configuration);
builder.Services.AddServices();

string[] roles = [Admin, Designer, Contributor, Client];
builder.Services.AddAuthWithCookie(builder.Configuration).AddRoles(roles);

builder.Services.AddMappings();
builder.Services.AddControllers().AddJsonAndXml();

builder.Services.AddApiConfigurations();
builder.Services.AddCorsForReact();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFilesAndCads();

await app.Services.UseCategoriesAsync().ConfigureAwait(false);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
await app.Services.UseRolesAsync(roles).ConfigureAwait(false);

if (app.Environment.IsDevelopment())
{
    await app.Services.UseAppUsers(app.Configuration, new()
    {
        [Admin] = "NinjataBG",
        [Designer] = "Designer",
        [Contributor] = "Contributor",
        [Client] = "Client",
    }).ConfigureAwait(false);
}

app.MapControllers();
app.MapFallbackToFile("index.html");
await app.RunAsync().ConfigureAwait(false);