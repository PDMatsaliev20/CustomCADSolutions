using static CustomCADs.Domain.DataConstants.RoleConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCadContext(builder.Configuration);
builder.Services.AddAppIdentity();
builder.Services.AddStripe(builder.Configuration);
builder.Services.AddServices();

string[] roles = [Admin, Designer, Contributor, Client];
builder.Services.AddAuthWithCookie().AddRoles(roles);

builder.Services.AddControllers().AddJsonAndXml();
builder.Services.AddApiConfigurations();
builder.Services.AddCorsForReact();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFilesAndCads();

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

Dictionary<string, string> users = new()
{
    [Admin] = "NinjataBG",
    [Designer] = "Designer",
    [Contributor] = "Contributor",
    [Client] = "Client",
};
await app.Services.UseAppUsers(app.Configuration, users).ConfigureAwait(false);

app.MapControllers();
await app.RunAsync().ConfigureAwait(false);