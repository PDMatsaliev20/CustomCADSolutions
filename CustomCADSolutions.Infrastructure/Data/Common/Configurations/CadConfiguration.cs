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
            builder.HasMany(c => c.Orders).WithOne(o => o.Cad).OnDelete(DeleteBehavior.NoAction);
         
            builder.Navigation(c => c.Creator).AutoInclude();
            builder.Navigation(c => c.Orders).AutoInclude();
            builder.Navigation(c => c.Category).AutoInclude();

            builder.HasData(new Cad()
            {
                Id = 1,
                Name = "Chair",
                CategoryId = 5,
                X = 750,
                Y = 300,
                Z = 0,
                SpinAxis = 'y',
                SpinFactor = -0.01,
            });
        }
    }
}
