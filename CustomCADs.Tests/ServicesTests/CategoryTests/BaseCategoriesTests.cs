using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Mappings;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Entities;
using CustomCADs.Persistence;
using CustomCADs.Persistence.Repositories;
using CustomCADs.Persistence.Repositories.Categories;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class BaseCategoriesTests
    {
        private readonly Persistence.ApplicationContext context = new(new DbContextOptionsBuilder<Persistence.ApplicationContext>()
            .UseInMemoryDatabase("CadCategoriesContext").Options);

        private readonly IMapper mapper = new MapperConfiguration(cfg => 
                cfg.AddProfile<CategoryProfile>())
            .CreateMapper();

        protected ICategoryService service;
        protected readonly CategoryModel[] categories =
        [
            new() { Id = 1, Name = "Category1" },
            new() { Id = 2, Name = "Category2" },
            new() { Id = 3, Name = "Category3" },
            new() { Id = 4, Name = "Category4" },
            new() { Id = 5, Name = "Category5" }
        ];

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            this.service = new CategoryService(new DbTracker(context),
                new CategoryQueries(context), 
                new CategoryCommands(context), 
                mapper);
        }

        [SetUp]
        public async Task Setup()
        {
            Category[] allCategories = mapper.Map<Category[]>(categories);
            await context.Categories.AddRangeAsync(allCategories).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        [TearDown]
        public async Task TearDown()
        {
            Category[] allCategories = await context.Categories.ToArrayAsync();
            context.Categories.RemoveRange(allCategories);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
