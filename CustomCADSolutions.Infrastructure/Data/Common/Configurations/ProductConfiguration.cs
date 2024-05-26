using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADSolutions.Infrastructure.Data.Common.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Navigation(p => p.Category).AutoInclude();
            builder.Navigation(p => p.Orders).AutoInclude();
            builder.Navigation(p => p.Category).AutoInclude();
            
            builder.HasMany(p => p.Orders).WithOne(o => o.Product).OnDelete(DeleteBehavior.NoAction);
            
            builder.Property(p => p.Price).HasPrecision(18, 2);
        }
    }
}
