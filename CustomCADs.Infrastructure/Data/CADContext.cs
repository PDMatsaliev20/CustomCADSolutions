using CustomCADs.Infrastructure.Data.Common.Configurations;
using CustomCADs.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data
{
    public class CadContext(DbContextOptions<CadContext> options) : IdentityDbContext<AppUser, AppRole, string>(options)
    {
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
