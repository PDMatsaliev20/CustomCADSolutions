using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADSolutions.Infrastructure.Data.Common
{
    public class CadConfiguration : IEntityTypeConfiguration<Cad>
    {
        public void Configure(EntityTypeBuilder<Cad> builder)
        {
            builder.HasData(Temp.ImportCads());
        }
    }
}
