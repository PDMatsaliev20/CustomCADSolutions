using CustomCADs.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Common.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Buyer).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.Navigation(o => o.Category).AutoInclude();
            builder.Navigation(o => o.Buyer).AutoInclude();
            builder.Navigation(o => o.Cad).AutoInclude();
        }
    }
}
