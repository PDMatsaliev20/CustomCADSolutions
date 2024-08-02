using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Buyer).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(o => o.Designer).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.Navigation(o => o.Category).AutoInclude();
            builder.Navigation(o => o.Buyer).AutoInclude();
            builder.Navigation(o => o.Designer).AutoInclude();
            builder.Navigation(o => o.Cad).AutoInclude();
        }
    }
}
