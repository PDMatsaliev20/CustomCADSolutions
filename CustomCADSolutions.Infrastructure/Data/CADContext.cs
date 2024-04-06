using CustomCADSolutions.Infrastructure.Data.Common.Configurations;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Infrastructure.Data
{
    public class CADContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public CADContext(DbContextOptions<CADContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Cad> Cads { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CadConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
