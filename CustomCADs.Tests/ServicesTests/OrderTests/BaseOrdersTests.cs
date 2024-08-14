using AutoMapper;
using CustomCADs.Core.Contracts;
using CustomCADs.Core.Mappings;
using CustomCADs.Core.Models.Orders;
using CustomCADs.Core.Services;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Entities.Identity;
using CustomCADs.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    [TestFixture]
    public class BaseOrdersTests
    {
        private IRepository repository;
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile<OrderProfile>())
            .CreateMapper();

        protected IOrderService service;
        protected AppUser[] users =
        [
            new() { UserName = "Client" },
            new() { UserName = "Contributor" },
            new() { UserName = "Hacker" },
        ];
        protected OrderModel[] orders =
        [
            new() { Id = 1, Name = "Order1", Description = "ClientOrder1", CategoryId = 1 },
            new() { Id = 2, Name = "Order2", Description = "ClientOrder2", CategoryId = 2 },
            new() { Id = 3, Name = "Order3", Description = "ClientOrder3", CategoryId = 3 },
            new() { Id = 4, Name = "Order4", Description = "ContributorOrder1", CategoryId = 4 },
            new() { Id = 5, Name = "Order5", Description = "ContributorOrder2", CategoryId = 5 },
        ];

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadOrdersContext").Options;
            
            this.repository = new Repository(new(options));
            SeedBuyers();
            
            await repository.AddRangeAsync(users).ConfigureAwait(false);
            await repository.AddRangeAsync(new Category[5]
            {
                new() { Id = 1, Name = "Category1", },
                new() { Id = 2, Name = "Category2", },
                new() { Id = 3, Name = "Category3", },
                new() { Id = 4, Name = "Category4", },
                new() { Id = 5, Name = "Category5", },
            }).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);

            this.service = new OrderService(repository, mapper);
        }

        [SetUp]
        public async Task Setup()
        {
            Order[] entities = mapper.Map<Order[]>(orders);
            await repository.AddRangeAsync(entities).ConfigureAwait(false);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        [TearDown]
        public async Task Teardown()
        {
            Order[] allOrders = await repository.All<Order>().ToArrayAsync();
            repository.DeleteRange(allOrders);
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTeardown()
        {
            Category[] categories = await repository.All<Category>().ToArrayAsync().ConfigureAwait(false);
            repository.DeleteRange(categories);

            AppUser[] users = await repository.All<AppUser>().ToArrayAsync().ConfigureAwait(false);
            repository.DeleteRange(users);

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        private void SeedBuyers()
        {
            for (int i = 0; i < 3; i++)
            {
                orders[i].BuyerId = users[0].Id;
            }
            for (int i = 3; i < 5; i++)
            {
                orders[i].BuyerId = users[1].Id;
            }
        }
    }
}
