using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.Core.Services;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class BaseCategoriesTests
    {
        private IRepository repository;

        protected ICategoryService service;
        protected readonly Category[] categories = new Category[5]
        {
            new() { Id = 1, Name = "Category1" },
            new() { Id = 2, Name = "Category2" },
            new() { Id = 3, Name = "Category3" },
            new() { Id = 4, Name = "Category4" },
            new() { Id = 5, Name = "Category5" }
        };

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadCategoriesContext").Options;

            this.repository = new Repository(new(options));
            this.service = new CategoryService(repository);
        }

        [SetUp]
        public async Task Setup()
        {
            await repository.AddRangeAsync(categories);
            await repository.SaveChangesAsync();
        }

        [TearDown]
        public async Task TearDown()
        {
            Category[] allCategories = await repository.All<Category>().ToArrayAsync();
            repository.DeleteRange(allCategories);
            await repository.SaveChangesAsync();
        }
    }
}
