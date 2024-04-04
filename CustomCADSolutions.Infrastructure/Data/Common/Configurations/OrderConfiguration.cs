using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADSolutions.Infrastructure.Data.Common.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasOne(o => o.Buyer).WithMany().OnDelete(DeleteBehavior.NoAction);
            builder.HasKey(o => new { o.CadId, o.BuyerId });
            builder.Navigation(o => o.Buyer).AutoInclude();
            builder.Navigation(o => o.Cad).AutoInclude();
        }
    }
}
