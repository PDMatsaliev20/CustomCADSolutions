using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data
{
    public class CustomCADSolutionsContext : DbContext
    {
        public CustomCADSolutionsContext(DbContextOptions<CustomCADSolutionsContext> options)
            : base(options)
        {
        }

        public DbSet<CAD> CADs { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
