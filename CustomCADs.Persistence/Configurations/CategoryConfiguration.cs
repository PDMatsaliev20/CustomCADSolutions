using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder
                .SetPrimaryKey()
                .SetValidations()
                .SetSeedData();
        }
    }

    static class CategoryConfigUtils
    {
        public static EntityTypeBuilder<Category> SetPrimaryKey(this EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(x => x.Id);

            return builder;
        }

        public static EntityTypeBuilder<Category> SetValidations(this EntityTypeBuilder<Category> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired();

            return builder;
        }

        public static EntityTypeBuilder<Category> SetSeedData(this EntityTypeBuilder<Category> builder)
        {
            string[] categoriesNames = 
            [
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
            ];

            int index = 0;
            builder.HasData(categoriesNames
                .Select(cn => new Category() { Id = ++index, Name = cn })
            );

            return builder;
        }
    }
}
