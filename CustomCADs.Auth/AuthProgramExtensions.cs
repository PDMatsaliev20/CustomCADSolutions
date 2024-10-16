using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using CustomCADs.Auth.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection;

public static class AuthProgramExtensions
{
    public static void AddIdentityContext(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = config.GetConnectionString("IdentityConnection")
                ?? throw new KeyNotFoundException("Could not find connection string 'IdentityConnection'.");
        services.AddDbContext<IdentityContext>(options => options.UseSqlServer(connectionString));
    }

    public static void AddIdentityAppManagers(this IServiceCollection services)
    {
        services.AddScoped<IAppUserManager, AppUserManager>();
        services.AddScoped<IAppRoleManager, AppRoleManager>();
    }
}
