using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Reads;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Reads;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using CustomCADs.Persistence;
using CustomCADs.Persistence.Repositories;
using CustomCADs.Persistence.Repositories.Reads;
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

    public static void AddReads(this IServiceCollection services)
    {
        services.AddScoped<IOrderReads, OrderReads>();
        services.AddScoped<ICadReads, CadReads>();
        services.AddScoped<ICategoryReads, CategoryReads>();
        services.AddScoped<IUserReads, UserReads>();
        services.AddScoped<IRoleReads, RoleReads>();
    }
    
    public static void AddWrites(this IServiceCollection services)
    {
        services.AddScoped(typeof(IWrites<>), typeof(Writes<>));
    }
}
