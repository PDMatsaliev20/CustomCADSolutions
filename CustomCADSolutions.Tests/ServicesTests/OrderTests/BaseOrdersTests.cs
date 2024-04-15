using AutoMapper;
using CustomCADSolutions.Core.Contracts;
using CustomCADSolutions.Core.Mappings;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Core.Services;
using CustomCADSolutions.Infrastructure.Data;
using CustomCADSolutions.Infrastructure.Data.Common;
using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    [TestFixture]
    public class BaseOrdersTests
    {
        private IRepository repository;
        private readonly IMapper mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile<OrderCoreProfile>())
            .CreateMapper();

        protected IOrderService service;
        protected AppUser[] users = new AppUser[3]
        {
            new() { UserName = "Client" },
            new() { UserName = "Contributor" },
            new() { UserName = "Hacker" },
        };
        protected OrderModel[] orders = new OrderModel[5]
        {
            new() { Id = 1, Name = "Order1", Description = "ClientOrder1", CategoryId = 1 },
            new() { Id = 2, Name = "Order2", Description = "ClientOrder2", CategoryId = 2 },
            new() { Id = 3, Name = "Order3", Description = "ClientOrder3", CategoryId = 3 },
            new() { Id = 4, Name = "Order4", Description = "ContributorOrder1", CategoryId = 4 },
            new() { Id = 5, Name = "Order5", Description = "ContributorOrder2", CategoryId = 5 },
        };

        [OneTimeSetUp]
        public async Task OneTimeSetup()
        {
            var options = new DbContextOptionsBuilder<CadContext>()
                .UseInMemoryDatabase("CadOrdersContext").Options;
            
            this.repository = new Repository(new(options));

            await repository.AddRangeAsync(users);
            SeedBuyers(orders, users);

            await repository.AddRangeAsync(new Category[5]
            {
                new() { Id = 1, Name = "Category1", },
                new() { Id = 2, Name = "Category2", },
                new() { Id = 3, Name = "Category3", },
                new() { Id = 4, Name = "Category4", },
                new() { Id = 5, Name = "Category5", },
            });
            await repository.SaveChangesAsync();

            service = new OrderService(repository);
        }

        [SetUp]
        public async Task Setup()
        {
            Order[] entities = mapper.Map<Order[]>(orders);
            await repository.AddRangeAsync(entities);
            await repository.SaveChangesAsync();
        }

        [TearDown]
        public async Task Teardown()
        {
            Order[] allOrders = await repository.All<Order>().ToArrayAsync();
            repository.DeleteRange(allOrders);
            await repository.SaveChangesAsync();
        }

        [OneTimeTearDown]
        public async Task OneTimeTeardown()
        {
            Category[] categories = await repository.All<Category>().ToArrayAsync();
            repository.DeleteRange(categories);

            AppUser[] users = await repository.All<AppUser>().ToArrayAsync();
            repository.DeleteRange(users);

            await repository.SaveChangesAsync();
        }

        private static void SeedBuyers(OrderModel[] orders, AppUser[] users)
        {
            orders[0].BuyerId = users[0].Id;
            orders[1].BuyerId = users[0].Id;
            orders[2].BuyerId = users[0].Id;
            orders[3].BuyerId = users[1].Id;
            orders[4].BuyerId = users[1].Id;
        }
    }
}
