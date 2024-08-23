using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            builder.HasOne(o => o.Buyer).WithMany()
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Designer).WithMany()
                .HasForeignKey(o => o.DesignerId)
                .OnDelete(DeleteBehavior.NoAction);
            
            builder.HasOne(o => o.Category).WithMany()
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
