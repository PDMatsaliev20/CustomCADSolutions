using CustomCADs.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Infrastructure.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<PCategory>
    {
        public void Configure(EntityTypeBuilder<PCategory> builder)
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

            PCategory[] categories = new PCategory[categoriesNames.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                categories[i] = new() { Id = i + 1, Name = categoriesNames[i] };
            }
            builder.HasData(categories);
        }
    }
}
