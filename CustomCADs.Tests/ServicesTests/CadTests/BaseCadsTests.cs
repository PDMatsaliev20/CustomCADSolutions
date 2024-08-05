using AutoMapper;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
using CustomCADs.Core.Models;
using CustomCADs.Core.Models.Cads;
using CustomCADs.Core.Services;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Enums;
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
            new() { Id = 1, Name = "Cad1", CategoryId = 1, Price = 1m, Status = CadStatus.Unchecked, CreationDate = DateTime.Now.AddDays(-1), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 2, Name = "Cad2", CategoryId = 2, Price = 2m, Status = CadStatus.Unchecked, CreationDate = DateTime.Now.AddDays(-2), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 3, Name = "Cad3", CategoryId = 3, Price = 3m, Status = CadStatus.Validated, CreationDate = DateTime.Now.AddDays(-3), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 4, Name = "Cad4", CategoryId = 4, Price = 4m, Status = CadStatus.Validated, CreationDate = DateTime.Now.AddDays(-4), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 5, Name = "Cad5", CategoryId = 5, Price = 5m, Status = CadStatus.Reported, CreationDate = DateTime.Now.AddDays(-5), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 6, Name = "Cad6", CategoryId = 6, Price = 6m, Status = CadStatus.Reported, CreationDate = DateTime.Now.AddDays(-6), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 7, Name = "Cad7", CategoryId = 7, Price = 7m, Status = CadStatus.Banned, CreationDate = DateTime.Now.AddDays(-7), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 8, Name = "Cad8", CategoryId = 8, Price = 8m, Status = CadStatus.Banned, CreationDate = DateTime.Now.AddDays(-8), Coords = new double[3], PanCoords = new double[3] },
        ];

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadCadsContext").Options;

            this.repository = new Repository(new(options));

            await repository.AddRangeAsync(users).ConfigureAwait(false);
            SeedCreators();

            await repository.AddRangeAsync(categories).ConfigureAwait(false);
            SeedCategories();
            
            await repository.SaveChangesAsync();

            this.service = new CadService(repository);
        }

        [SetUp]
        public async Task Setup()
        {
            Cad[] allCads = mapper.Map<Cad[]>(cads);
            await repository.AddRangeAsync<Cad>(allCads).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        [TearDown]
        public async Task Teardown()
        {
            Cad[] cads = await repository.All<Cad>().ToArrayAsync().ConfigureAwait(false);
            repository.DeleteRange(cads);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            AppUser[] users = await repository.All<AppUser>().ToArrayAsync().ConfigureAwait(false);
            repository.DeleteRange(users);
            
            Category[] categories = await repository.All<Category>().ToArrayAsync().ConfigureAwait(false);
            repository.DeleteRange(categories);
            
            await repository.SaveChangesAsync().ConfigureAwait(false);
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
