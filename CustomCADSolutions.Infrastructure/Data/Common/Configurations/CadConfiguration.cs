using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data.Common.Configurations
{
    public class CadConfiguration : IEntityTypeConfiguration<Cad>
    {
        public void Configure(EntityTypeBuilder<Cad> builder)
        {
            builder.Navigation(c => c.Creator).AutoInclude();
            builder.Navigation(c => c.Orders).AutoInclude();
            builder.Navigation(c => c.Category).AutoInclude();
        }
    }
}
