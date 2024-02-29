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
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            string[] categoriesNames = new[]
            {
                "Animals",
                "Characters",
                "Electronics",
                "Fashion",
                "Furniture",
                "Nature",
                "Science",
                "Sports",
                "Toys",
                "Vehicles",
                "Others",
            };

            string[] categoriesNamesBg = new[]
           {
                "Животни",
                "Герои",
                "Електроника",
                "Мода",
                "Мебели",
                "Природа",
                "Наука",
                "Спорт",
                "Играчки",
                "Коли",
                "Други",
            };

            Category[] categories = new Category[categoriesNames.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                categories[i] = new() { Id = i + 1, Name = categoriesNames[i], BgName = categoriesNamesBg[i] };
            }
            builder.HasData(categories);
        }
    }
}
