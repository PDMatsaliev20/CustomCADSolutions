using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Persistence;
using CustomCADs.Persistence.Repositories;
using CustomCADs.Persistence.Repositories.Cads;
using CustomCADs.Persistence.Repositories.Categories;
using CustomCADs.Persistence.Repositories.Orders;
using CustomCADs.Persistence.Repositories.Roles;
using CustomCADs.Persistence.Repositories.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

#pragma warning disable IDE0130
namespace Microsoft.Extensions.DependencyInjection;

public static class PersistenceProgramExtensions
{
    public static void AddApplicationContext(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = config.GetConnectionString("RealConnection")
                ?? throw new KeyNotFoundException("Could not find connection string 'RealConnection'.");
        services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    public static void AddQueries(this IServiceCollection services)
    {
        services.AddScoped<IOrderQueries, OrderQueries>();
        services.AddScoped<ICadQueries, CadQueries>();
        services.AddScoped<ICategoryQueries, CategoryQueries>();
        services.AddScoped<IUserQueries, UserQueries>();
        services.AddScoped<IRoleQueries, RoleQueries>();
    }
    
    public static void AddCommands(this IServiceCollection services)
    {
        services.AddScoped<ICommands<Order>, OrderCommands>();
        services.AddScoped<ICommands<Cad>, CadCommands>();
        services.AddScoped<ICommands<Category>, CategoryCommands>();
        services.AddScoped<ICommands<User>, UserCommands>();
        services.AddScoped<ICommands<Role>, RoleCommands>();
    }
}
