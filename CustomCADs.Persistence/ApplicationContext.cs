using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Persistence;

public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
{
    public DbSet<Category> Categories { get; set; }
    public DbSet<Cad> Cads { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationContext).Assembly);
    }
}
