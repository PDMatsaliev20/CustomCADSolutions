#pragma warning disable IDE0130
using CustomCADs.Application;

namespace Microsoft.Extensions.DependencyInjection;

public static class ApplicationProgramExtensions
{
    public static void AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ApplicationReference>());
    }

    public static void AddApplicationMappings(this IServiceCollection services)
    {

    }
}
