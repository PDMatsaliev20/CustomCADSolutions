using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Persistence.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).ValueGeneratedOnAdd();

            builder
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasPrincipalKey(u => u.Name)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
