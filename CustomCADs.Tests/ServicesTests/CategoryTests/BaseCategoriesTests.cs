using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Mappings;
using CustomCADs.Application.Models;
using CustomCADs.Application.Services;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class BaseCategoriesTests
    {
        private IRepository repository;
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
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadCategoriesContext").Options;

            this.repository = new Repository(new(options));
            this.service = new CategoryService(repository, mapper);
        }

        [SetUp]
        public async Task Setup()
        {
            Category[] categories = mapper.Map<Category[]>(this.categories);
            await repository.AddRangeAsync(categories).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        [TearDown]
        public async Task TearDown()
        {
            Category[] allCategories = await repository.All<Category>().ToArrayAsync();
            repository.DeleteRange(allCategories);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
