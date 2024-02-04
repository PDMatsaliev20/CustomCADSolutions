using CustomCADSolutions.Infrastructure.Data.Configuration;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Infrastructure.Data
{
    public class CustomCADSolutionsContext : DbContext
    {
        public CustomCADSolutionsContext(DbContextOptions<CustomCADSolutionsContext> options)
            : base(options)
        {
        }

        public DbSet<Cad> Cads { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CadConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
