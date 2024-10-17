using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CustomCADs.Domain.DataConstants.CadConstants;

namespace CustomCADs.Persistence.Configurations;

public class CadConfiguration : IEntityTypeConfiguration<Cad>
{
    public void Configure(EntityTypeBuilder<Cad> builder)
    {
        builder
            .SetPrimaryKey()
            .SetForeignKeys()
            .SetValueObjects()
            .SetValidations();
    }
}

static class CadConfigUtils
{
    public static EntityTypeBuilder<Cad> SetPrimaryKey(this EntityTypeBuilder<Cad> builder)
    {
        builder.HasKey(x => x.Id);

        return builder;
    }

    public static EntityTypeBuilder<Cad> SetForeignKeys(this EntityTypeBuilder<Cad> builder)
    {
        builder
            .HasOne(c => c.Category).WithMany()
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.NoAction);

        builder
            .HasOne(c => c.Creator).WithMany()
            .HasForeignKey(c => c.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        return builder;
    }
    
    public static EntityTypeBuilder<Cad> SetValueObjects(this EntityTypeBuilder<Cad> builder)
    {
        builder.OwnsOne(c => c.CamCoordinates, cb =>
        {
            cb.Property(c => c.X).IsRequired().HasColumnName("CamCoordX");
            cb.Property(c => c.Y).IsRequired().HasColumnName("CamCoordY");
            cb.Property(c => c.Z).IsRequired().HasColumnName("CamCoordZ");
        });

        builder.OwnsOne(c => c.PanCoordinates, cb =>
        {
            cb.Property(c => c.X).IsRequired().HasColumnName("PanCoordX");
            cb.Property(c => c.Y).IsRequired().HasColumnName("PanCoordY");
            cb.Property(c => c.Z).IsRequired().HasColumnName("PanCoordZ");
        });

        builder.OwnsOne(c => c.Paths, cb =>
        {
            cb.Property(c => c.ImagePath).IsRequired().HasColumnName("ImagePath");
            cb.Property(c => c.FilePath).IsRequired().HasColumnName("FilePath");
        });

        return builder;
    }

    public static EntityTypeBuilder<Cad> SetValidations(this EntityTypeBuilder<Cad> builder)
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(NameMaxLength);
        
        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(DescriptionMaxLength);
        
        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion(
                e => e.ToString(),
                s => Enum.Parse<CadStatus>(s)
            );

        builder.Property(c => c.Price)
            .IsRequired()
            .HasPrecision(18, 2);
        
        builder.Property(c => c.CreationDate)
            .IsRequired();
        
        builder.Property(c => c.CategoryId)
            .IsRequired();
        
        builder.Property(c => c.CreatorId)
            .IsRequired();

        return builder;
    }
}
