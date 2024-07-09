using CustomCADs.Infrastructure.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Common.Configurations
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

            Category[] categories = new Category[categoriesNames.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                categories[i] = new() { Id = i + 1, Name = categoriesNames[i] };
            }
            builder.HasData(categories);
        }
    }
}
