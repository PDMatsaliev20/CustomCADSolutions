using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADSolutions.Infrastructure.Data.Common.Configurations
{
    public class CadConfiguration : IEntityTypeConfiguration<Cad>
    {
        public void Configure(EntityTypeBuilder<Cad> builder)
        {
            builder.HasOne(c => c.Creator).WithMany().OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(c => c.Product).WithOne(p => p.Cad).OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(c => c.Creator).AutoInclude();
            builder.Navigation(c => c.Product).AutoInclude();
        }
    }
}
