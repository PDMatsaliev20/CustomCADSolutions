using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Configurations
{
    public class CadConfiguration : IEntityTypeConfiguration<Cad>
    {
        public void Configure(EntityTypeBuilder<Cad> builder)
        {
            builder.HasOne(c => c.Creator).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.HasMany(c => c.Orders).WithOne(o => o.Cad).OnDelete(DeleteBehavior.NoAction);

            builder.Navigation(c => c.Creator).AutoInclude();
            builder.Navigation(c => c.Orders).AutoInclude();
            builder.Navigation(c => c.Category).AutoInclude();

            builder.Property(c => c.Price).HasPrecision(18, 2);
        }
    }
}
