using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADSolutions.Infrastructure.Data.Common
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Navigation(o => o.Cad).AutoInclude();
            builder.Navigation(o => o.Buyer).AutoInclude();
        }
    }
}
