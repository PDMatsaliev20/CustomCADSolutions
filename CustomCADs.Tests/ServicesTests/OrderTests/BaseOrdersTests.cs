using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Mappings;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Services;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Identity;
using CustomCADs.Infrastructure.Data;
using CustomCADs.Infrastructure.Data.Repositories.Command;
using CustomCADs.Infrastructure.Data.Repositories.Query;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    [TestFixture]
    public class BaseOrdersTests
    {
        private readonly CadContext context = new(new DbContextOptionsBuilder<CadContext>().UseInMemoryDatabase("CadOrdersContext").Options);
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
            await context.Users.AddRangeAsync(users).ConfigureAwait(false);
            await context.Categories.AddRangeAsync(
            [
                new() { Id = 1, Name = "Category1", },
                new() { Id = 2, Name = "Category2", },
                new() { Id = 3, Name = "Category3", },
                new() { Id = 4, Name = "Category4", },
                new() { Id = 5, Name = "Category5", },
            ]).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);

            SeedBuyers();
            this.service = new OrderService(
                new OrderQueryRepository(context), 
                new OrderCommandRepository(context), 
                mapper);
        }

        [SetUp]
        public async Task Setup()
        {
            Order[] entities = mapper.Map<Order[]>(orders);
            await context.Orders.AddRangeAsync(entities).ConfigureAwait(false);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        [TearDown]
        public async Task Teardown()
        {
            Order[] allOrders = await context.Orders.ToArrayAsync();
            context.Orders.RemoveRange(allOrders);
            await context.SaveChangesAsync().ConfigureAwait(false);
        }

        [OneTimeTearDown]
        public async Task OneTimeTeardown()
        {
            Category[] categories = await context.Categories.ToArrayAsync().ConfigureAwait(false);
            context.Categories.RemoveRange(categories);

            AppUser[] users = await context.Users.ToArrayAsync().ConfigureAwait(false);
            context.Users.RemoveRange(users);

            await context.SaveChangesAsync().ConfigureAwait(false);
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
