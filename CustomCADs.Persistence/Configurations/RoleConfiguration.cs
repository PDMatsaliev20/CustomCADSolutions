using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CustomCADs.Domain.DataConstants.RoleConstants;

namespace CustomCADs.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder
            .SetPrimaryKey()
            .SetForeignKeys()
            .SetValidations();
    }
}


static class RoleConfigUtils
{
    public static EntityTypeBuilder<Role> SetPrimaryKey(this EntityTypeBuilder<Role> builder) 
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).ValueGeneratedOnAdd();

        return builder;
    }

    public static EntityTypeBuilder<Role> SetForeignKeys(this EntityTypeBuilder<Role> builder) 
    {
        builder
            .HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasPrincipalKey(u => u.Name)
            .OnDelete(DeleteBehavior.NoAction);

        return builder;
    }
    
    public static EntityTypeBuilder<Role> SetValidations(this EntityTypeBuilder<Role> builder) 
    {
        builder.Property(c => c.Name)
            .IsRequired()
            .HasMaxLength(NameMaxLength);
        
        builder.Property(c => c.Description)
            .IsRequired()
            .HasMaxLength(DescriptionMaxLength);

        return builder;
    }
}
