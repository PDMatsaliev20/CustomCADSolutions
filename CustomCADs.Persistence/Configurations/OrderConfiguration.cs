using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CustomCADs.Domain.DataConstants.OrderConstants;

namespace CustomCADs.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder
                .SetPrimaryKey()
                .SetForeignKeys()
                .SetValidations();
        }
    }

    static class OrderConfigUtils
    {
        public static EntityTypeBuilder<Order> SetPrimaryKey(this EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(o => o.Id);

            return builder;
        }

        public static EntityTypeBuilder<Order> SetForeignKeys(this EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Buyer).WithMany()
                .HasForeignKey(o => o.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(o => o.Designer).WithMany()
                .HasForeignKey(o => o.DesignerId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(o => o.Category).WithMany()
                .HasForeignKey(o => o.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            return builder;
        }
        
        public static EntityTypeBuilder<Order> SetValidations(this EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.Name)
                .IsRequired()
                .HasMaxLength(NameMaxLength);
            
            builder.Property(o => o.Description)
                .IsRequired()
                .HasMaxLength(DescriptionMaxLength);
            
            builder.Property(o => o.OrderDate)
                .IsRequired();
            
            builder.Property(o => o.Status)
                .IsRequired();
            
            builder.Property(o => o.ShouldBeDelivered)
                .IsRequired();
            
            builder.Property(o => o.ImagePath)
                .IsRequired();
            
            builder.Property(o => o.CategoryId)
                .IsRequired();
            
            builder.Property(o => o.BuyerId)
                .IsRequired();

            return builder;
        }

    }
}
