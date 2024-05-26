using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADSolutions.Infrastructure.Data.Common.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Navigation(o => o.Category).AutoInclude();
            builder.Navigation(o => o.Buyer).AutoInclude();
            builder.Navigation(o => o.Product).AutoInclude();

            builder.HasOne(o => o.Buyer).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
