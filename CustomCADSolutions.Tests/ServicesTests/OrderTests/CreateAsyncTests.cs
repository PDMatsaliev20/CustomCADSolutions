using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    public class CreateAsyncTests : BaseOrdersTests
    {
        [Test]
        public async Task Test_AddsCorrectly()
        {
            OrderModel expected = new()
            {
                Name = "NewOrder",
                Description = "NewContributorOrder",
                CategoryId = 3,
                BuyerId = users[1].Id,
            };
            int id = await service.CreateAsync(expected);

            OrderModel actual = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(expected.Name), "Name mismatch.");
                Assert.That(actual.Description, Is.EqualTo(expected.Description), "Description mismatch.");
                Assert.That(actual.CategoryId, Is.EqualTo(expected.CategoryId), "CategoryId mismatch.");
                Assert.That(actual.BuyerId, Is.EqualTo(expected.BuyerId), "BuyerId mismatch.");
            });
        }

        [Test]
        public void Test_ThrowsWhenOrderIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(null!);
            }, "Created null Order.");
        }
    }
}
