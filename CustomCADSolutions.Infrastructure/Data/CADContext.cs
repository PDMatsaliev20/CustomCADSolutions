using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Infrastructure.Data
{
    public class CADContext : IdentityDbContext
    {
        public CADContext(DbContextOptions<CADContext> options)
            : base(options)
        {
        }

        public DbSet<Cad> Cads { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>()
                .HasKey(o => new { o.CadId, o.BuyerId });
            
            modelBuilder.Entity<Order>().Navigation(o => o.Buyer).AutoInclude();
            modelBuilder.Entity<Order>().Navigation(o => o.Cad).AutoInclude();

            modelBuilder.Entity<Cad>().Navigation(c => c.Creator).AutoInclude();
            modelBuilder.Entity<Cad>().Navigation(c => c.Orders).AutoInclude();

            base.OnModelCreating(modelBuilder);
        }
    }
}
