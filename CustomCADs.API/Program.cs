using static CustomCADs.Domain.DataConstants.RoleConstants;

var builder = WebApplication.CreateBuilder(args);

// Add Application and Identity contexts
builder.Services.AddApplicationContext(builder.Configuration);
builder.Services.AddIdentityContext(builder.Configuration);

// Add External services
builder.Services.AddStripe(builder.Configuration);
builder.Services.AddEmail(builder.Configuration);

// Add Core services
builder.Services.AddMediator();
builder.Services.AddServices();

// Add Authentication and Authorization with JWT in a Cookie
string[] roles = [Admin, Designer, Contributor, Client];
builder.Services.AddAuthWithCookie(builder.Configuration).AddRoles(roles);

// Add Mapper classes/profiles
builder.Services.AddMappings();

// Add API/Endpoints + configs
builder.Services.AddEndpoints().AddJsonAndXml();
builder.Services.AddApiConfigurations();
builder.WebHost.AddUploadSizeLimitations();

// Add CORS for Client
builder.Services.AddCorsForReact(builder.Configuration);

WebApplication app = builder.Build();

// Use Server files
app.UseDefaultFiles();
app.UseStaticFilesAndCads();

// Use Swagger + UI if in Dev mode
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use HTTPS and CORS
app.UseHttpsRedirection();
app.UseCors();

// Use Exception filter
app.UseGlobalExceptionHandler();

// Use AuthN and AuthZ
app.UseAuthentication();
app.UseAuthorization();
await app.Services.UseRolesAsync(roles).ConfigureAwait(false);

// Use Endpoints and a Fallback File
app.UseEndpoints();
app.MapFallbackToFile("index.html");

await app.RunAsync().ConfigureAwait(false);