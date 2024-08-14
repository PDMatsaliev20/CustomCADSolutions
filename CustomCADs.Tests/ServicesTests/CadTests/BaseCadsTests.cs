using AutoMapper;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
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
            {
                cfg.AddProfile<CadProfile>();
                cfg.AddProfile<CategoryProfile>();
            }).CreateMapper();

        protected ICadService service;
        private Category[] categories =
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
            new() { Id = 1, Name = "Cad1", Description="abcdef", CategoryId = 1, Price = 1m, Status = CadStatus.Unchecked, CreationDate = DateTime.Now.AddDays(-1), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 2, Name = "Cad2", Description="ghijklm", CategoryId = 2, Price = 2m, Status = CadStatus.Unchecked, CreationDate = DateTime.Now.AddDays(-2), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 3, Name = "Cad3", Description="oprqstn", CategoryId = 3, Price = 3m, Status = CadStatus.Validated, CreationDate = DateTime.Now.AddDays(-3), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 4, Name = "Cad4", Description="uvwxyz", CategoryId = 4, Price = 4m, Status = CadStatus.Validated, CreationDate = DateTime.Now.AddDays(-4), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 5, Name = "Cad5", Description="abvgde", CategoryId = 5, Price = 5m, Status = CadStatus.Reported, CreationDate = DateTime.Now.AddDays(-5), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 6, Name = "Cad6", Description="zijklmn", CategoryId = 6, Price = 6m, Status = CadStatus.Reported, CreationDate = DateTime.Now.AddDays(-6), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 7, Name = "Cad7", Description="oprstufh", CategoryId = 7, Price = 7m, Status = CadStatus.Banned, CreationDate = DateTime.Now.AddDays(-7), Coords = new double[3], PanCoords = new double[3] },
            new() { Id = 8, Name = "Cad8", Description="cchshsht", CategoryId = 8, Price = 8m, Status = CadStatus.Banned, CreationDate = DateTime.Now.AddDays(-8), Coords = new double[3], PanCoords = new double[3] },
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
            await repository.SaveChangesAsync();

            this.service = new CadService(repository, mapper);
        }

        [SetUp]
        public async Task Setup()
        {
            Cad[] allCads = mapper.Map<Cad[]>(cads);
            await repository.AddRangeAsync<Cad>(allCads).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);
            cads = mapper.Map<CadModel[]>(allCads);
        }

        [TearDown]
        public async Task Teardown()
        {
            ClearCadCategories();
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

        private void ClearCadCategories()
        {
            foreach (CadModel cad in cads)
            {
                cad.Category = null!;
            }
        }
    }
}
