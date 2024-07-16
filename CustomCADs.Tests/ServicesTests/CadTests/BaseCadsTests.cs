using AutoMapper;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
using CustomCADs.Core.Models;
using CustomCADs.Core.Services;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Identity;
using CustomCADs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    [TestFixture]
    public class BaseCadsTests
    {
        private IRepository repository;
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile<CadCoreProfile>())
            .CreateMapper();

        protected ICadService service;
        private CategoryModel[] categories = 
        [
            new() { Id = 1, Name = "Category1" },
            new() { Id = 2, Name = "Category2" },
            new() { Id = 3, Name = "Category3" },
            new() { Id = 4, Name = "Category4" },
            new() { Id = 5, Name = "Category5" },
            new() { Id = 6, Name = "Category6" },
            new() { Id = 7, Name = "Category7" },
            new() { Id = 8, Name = "Category8" },
        ];
        protected AppUser[] users =
        [
            new() { UserName = "Contributor" },
            new() { UserName = "Designer" },
            new() { UserName = "Hacker" },
        ];
        protected CadModel[] cads =
        [
            new() { Id = 1, Name = "Cad1", Extension = "a", CategoryId = 1, Price = 1m, IsValidated = false, CreationDate = DateTime.Now.AddDays(-1), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 2, Name = "Cad2", Extension = "ab", CategoryId = 2, Price = 2m, IsValidated = false, CreationDate = DateTime.Now.AddDays(-2), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 3, Name = "Cad3", Extension = "abc", CategoryId = 3, Price = 3m, IsValidated = false, CreationDate = DateTime.Now.AddDays(-3), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 4, Name = "Cad4", Extension = "abcd", CategoryId = 4, Price = 4m, IsValidated = false, CreationDate = DateTime.Now.AddDays(-4), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 5, Name = "Cad5", Extension = "abcde", CategoryId = 5, Price = 5m, IsValidated = false, CreationDate = DateTime.Now.AddDays(-5), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 6, Name = "Cad6", Extension = "abcdef", CategoryId = 6, Price = 6m, IsValidated = true, CreationDate = DateTime.Now.AddDays(-6), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 7, Name = "Cad7", Extension = "abcdefg", CategoryId = 7, Price = 7m, IsValidated = true, CreationDate = DateTime.Now.AddDays(-7), Coords = new int[3], PanCoords = new int[3] },
            new() { Id = 8, Name = "Cad8", Extension = "abcdefgh", CategoryId = 8, Price = 8m, IsValidated = true, CreationDate = DateTime.Now.AddDays(-8), Coords = new int[3], PanCoords = new int[3] },
        ];

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadCadsContext").Options;

            this.repository = new Repository(new(options));

            await repository.AddRangeAsync(users);
            SeedCreators();

            await repository.AddRangeAsync(categories);
            SeedCategories();
            
            await repository.SaveChangesAsync();

            this.service = new CadService(repository);
        }

        [SetUp]
        public async Task Setup()
        {
            Cad[] allCads = mapper.Map<Cad[]>(cads);
            await repository.AddRangeAsync<Cad>(allCads);
            await repository.SaveChangesAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            Cad[] cads = await repository.All<Cad>().ToArrayAsync();
            repository.DeleteRange(cads);
            await repository.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            AppUser[] users = await repository.All<AppUser>().ToArrayAsync();
            repository.DeleteRange(users);
            
            Category[] categories = await repository.All<Category>().ToArrayAsync();
            repository.DeleteRange(categories);
            
            await repository.SaveChangesAsync();
        }

        private void SeedCreators()
        {
            for (int i = 0; i < 5; i++)
            {
                cads[i].CreatorId = users[0].Id;
                cads[i].Creator = users[0];
            }
            
            for (int i = 5; i < 8; i++)
            {
                cads[i].CreatorId = users[1].Id;
                cads[i].Creator = users[1];
            }
        }
        
        private void SeedCategories()
        {
            for (int i = 0; i < cads.Length; i++)
            {
                cads[i].CategoryId = categories[i].Id;
                cads[i].Category = categories[i];
            }
        }
    }
}
