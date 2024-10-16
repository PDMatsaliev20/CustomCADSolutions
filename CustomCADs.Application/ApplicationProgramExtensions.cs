#pragma warning disable IDE0130
using CustomCADs.Application;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Services;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationProgramExtensions
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationReference>());
    }

    public static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
    }

    public static void AddApplicationMappings(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ApplicationReference));
    }
}
