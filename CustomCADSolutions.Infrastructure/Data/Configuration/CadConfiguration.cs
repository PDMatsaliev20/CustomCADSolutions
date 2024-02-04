using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CustomCADSolutions.Infrastructure.Data.Configuration
{
    public class CadConfiguration : IEntityTypeConfiguration<Cad>
    {
        public void Configure(EntityTypeBuilder<Cad> builder)
        {
            string json = File.ReadAllText("categories.json");
            Cad[] cads = JsonSerializer.Deserialize<Cad[]>(json)!;
            
            int i = 0;
            while (i < cads.Length)
            {
                cads[i].Id = ++i;
            }
            
            builder.HasData(cads);
        }
    }
}
