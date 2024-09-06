using CustomCADs.Infrastructure.Data.Identity;
using CustomCADs.Infrastructure.Data.Configurations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CustomCADs.Infrastructure.Data.Entities;

namespace CustomCADs.Infrastructure.Data
{
    public class CadContext(DbContextOptions<CadContext> options) : IdentityDbContext<AppUser, AppRole, string>(options)
    {
        public DbSet<POrder> Orders { get; set; } = null!;
        public DbSet<PCad> Cads { get; set; } = null!;
        public DbSet<PCategory> Categories { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new CadConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
