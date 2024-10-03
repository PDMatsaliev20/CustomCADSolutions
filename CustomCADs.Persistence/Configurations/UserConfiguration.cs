using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static CustomCADs.Domain.DataConstants.UserConstants;

namespace CustomCADs.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder
                .SetPrimaryKey()
                .SetForeignKeys()
                .SetValidations();
        }
    }

    static class UserConfigUtils
    {
        public static EntityTypeBuilder<User> SetPrimaryKey(this EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            return builder;
        }

        public static EntityTypeBuilder<User> SetForeignKeys(this EntityTypeBuilder<User> builder)
        {
            builder
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleName)
                .OnDelete(DeleteBehavior.Restrict);

            return builder;
        }
        
        public static EntityTypeBuilder<User> SetValidations(this EntityTypeBuilder<User> builder)
        {
            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(NameMaxLength);
            
            builder.Property(u => u.Email)
                .IsRequired();
            
            builder.Property(u => u.FirstName)
                .HasMaxLength(NameMaxLength);
            
            builder.Property(u => u.LastName)
                .HasMaxLength(NameMaxLength);
            
            builder.Property(u => u.RoleName)
                .IsRequired();
            
            return builder;
        }

    }
}
