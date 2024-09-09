﻿using CustomCADs.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomCADs.Domain.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
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

            Category[] categories = new Category[categoriesNames.Length];
            for (int i = 0; i < categories.Length; i++)
            {
                categories[i] = new() { Id = i + 1, Name = categoriesNames[i] };
            }
            builder.HasData(categories);
        }
    }
}
