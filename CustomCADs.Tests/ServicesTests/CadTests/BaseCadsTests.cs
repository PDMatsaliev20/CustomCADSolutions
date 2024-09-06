using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Mappings;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using CustomCADs.Infrastructure.Data;
using CustomCADs.Infrastructure.Data.Entities;
using CustomCADs.Infrastructure.Data.Identity;
using CustomCADs.Infrastructure.Data.Repositories;
using CustomCADs.Infrastructure.Data.Repositories.Command;
using CustomCADs.Infrastructure.Data.Repositories.Query;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    [TestFixture]
    public class BaseCadsTests
    {
        private readonly CadContext context = new(new DbContextOptionsBuilder<CadContext>()
            .UseInMemoryDatabase("CadCadsContext").Options);
        
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CadProfile>();
                cfg.AddProfile<CategoryProfile>();
            }).CreateMapper();

        protected ICadService service;
        private readonly Category[] categories =
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
            new("Contributor"),
            new("Designer"),
            new("Hacker"),
        ];
        protected CadModel[] cads =
        [
            new() { Id = 1, Name = "Cad1", Description="abcdef", CategoryId = 1, Price = 1m, Status = CadStatus.Unchecked, CreationDate = DateTime.Now.AddDays(-1) },
            new() { Id = 2, Name = "Cad2", Description="ghijklm", CategoryId = 2, Price = 2m, Status = CadStatus.Unchecked, CreationDate = DateTime.Now.AddDays(-2) },
            new() { Id = 3, Name = "Cad3", Description="oprqstn", CategoryId = 3, Price = 3m, Status = CadStatus.Validated, CreationDate = DateTime.Now.AddDays(-3) },
            new() { Id = 4, Name = "Cad4", Description="uvwxyz", CategoryId = 4, Price = 4m, Status = CadStatus.Validated, CreationDate = DateTime.Now.AddDays(-4) },
            new() { Id = 5, Name = "Cad5", Description="abvgde", CategoryId = 5, Price = 5m, Status = CadStatus.Reported, CreationDate = DateTime.Now.AddDays(-5) },
            new() { Id = 6, Name = "Cad6", Description="zijklmn", CategoryId = 6, Price = 6m, Status = CadStatus.Reported, CreationDate = DateTime.Now.AddDays(-6) },
            new() { Id = 7, Name = "Cad7", Description="oprstufh", CategoryId = 7, Price = 7m, Status = CadStatus.Banned, CreationDate = DateTime.Now.AddDays(-7) },
            new() { Id = 8, Name = "Cad8", Description="cchshsht", CategoryId = 8, Price = 8m, Status = CadStatus.Banned, CreationDate = DateTime.Now.AddDays(-8) },
        ];

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            await context.Users.AddRangeAsync(users).ConfigureAwait(false);
            SeedCreators();
            await context.Categories.AddRangeAsync(mapper.Map<PCategory[]>(categories)).ConfigureAwait(false);
            await context.SaveChangesAsync();

            service = new CadService(new DbTracker(context),
                new CadQueryRepository(context, mapper),
                new OrderQueryRepository(context, mapper),
                new CadCommandRepository(context, mapper),
                mapper);
        }

        [SetUp]
        public async Task Setup()
        {
            Cad[] allCads = mapper.Map<Cad[]>(cads);
            await context.Cads.AddRangeAsync(mapper.Map<PCad[]>(allCads)).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
            cads = mapper.Map<CadModel[]>(allCads);
        }

        [TearDown]
        public async Task Teardown()
        {
            ClearCadCategories();
            PCad[] cads = await context.Cads.ToArrayAsync().ConfigureAwait(false);
            context.Cads.RemoveRange(cads);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTearDown()
        {
            AppUser[] users = await context.Users.ToArrayAsync().ConfigureAwait(false);
            context.Users.RemoveRange(users);

            PCategory[] categories = await context.Categories.ToArrayAsync().ConfigureAwait(false);
            context.Categories.RemoveRange(categories);

            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        private void SeedCreators()
        {
            for (int i = 0; i < 5; i++)
            {
                cads[i].CreatorId = users[0].Id;
                cads[i].Creator = mapper.Map<User>(users[0]);
            }

            for (int i = 5; i < 8; i++)
            {
                cads[i].CreatorId = users[1].Id;
                cads[i].Creator = mapper.Map<User>(users[1]);
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
