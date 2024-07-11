using static CustomCADs.Infrastructure.Data.DataConstants.RoleConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCadContext(builder.Configuration);
builder.Services.AddAppIdentity();
builder.Services.AddAbstractions();

string[] roles = [Admin, Designer, Contributor, Client];
builder.Services.AddAuthWithCookie().AddRoles(roles);

builder.Services.AddControllers().AddJsonAndXml();
builder.Services.AddApiConfigurations();
builder.Services.AddCorsForReact();

WebApplication app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();

app.UseAuthentication();
app.UseAuthorization();
await app.Services.UseRolesAsync(roles);

Dictionary<string, string> users = new()
{
    [Admin] = "NinjataBG",
    [Designer] = "Designer",
    [Contributor] = "Contributor",
    [Client] = "Client",
};
await app.Services.UseAppUsers(app.Configuration, users);

app.MapControllers();
await app.RunAsync();