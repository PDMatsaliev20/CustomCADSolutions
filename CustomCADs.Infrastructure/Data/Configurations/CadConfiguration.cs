using CustomCADs.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Configurations
{
    public class CadConfiguration : IEntityTypeConfiguration<PCad>
    {
        public void Configure(EntityTypeBuilder<PCad> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(c => c.Category).WithMany()
                .HasForeignKey(c => c.CategoryId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(c => c.Creator).WithMany()
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Orders).WithOne(o => o.Cad)
                .OnDelete(DeleteBehavior.NoAction);

            builder.OwnsOne(c => c.CamCoordinates, cb =>
            {
                cb.Property(c => c.X).HasColumnName("CamCoordX");
                cb.Property(c => c.Y).HasColumnName("CamCoordY");
                cb.Property(c => c.Z).HasColumnName("CamCoordZ");
            });
            
            builder.OwnsOne(c => c.PanCoordinates, cb =>
            {
                cb.Property(c => c.X).HasColumnName("PanCoordX");
                cb.Property(c => c.Y).HasColumnName("PanCoordY");
                cb.Property(c => c.Z).HasColumnName("PanCoordZ");
            });
            
            builder.OwnsOne(c => c.Paths, cb =>
            {
                cb.Property(c => c.ImagePath).HasColumnName("ImagePath");
                cb.Property(c => c.FilePath).HasColumnName("FilePath");
            });

            builder.Property(c => c.Price).HasPrecision(18, 2);
        }
    }
}
