using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;
using CustomCADSolutions.Core.Services;

namespace CustomCADSolutions.Tests.ServicesTests.CategoryTests
{
    public class BaseTests
    {
        private CadContext context;
        protected ICategoryService service;
        protected readonly Category[] categories = new Category[5]
        {
            new() { Id = 1, Name = "Category1" },
            new() { Id = 2, Name = "Category2" },
            new() { Id = 3, Name = "Category3" },
            new() { Id = 4, Name = "Category4" },
            new() { Id = 5, Name = "Category5" }
        };

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadSo%lutionsContext").Options;

            context = new(options);
            context.Categories.AddRange(categories);
            context.SaveChanges();

            IRepository repository = new Repository(context);
            service = new CategoryService(repository);
        }

        [TearDown]
        public void TearDown()
        {
            context.Categories.RemoveRange(context.Categories);
            context.SaveChanges();
        }
    }
}
