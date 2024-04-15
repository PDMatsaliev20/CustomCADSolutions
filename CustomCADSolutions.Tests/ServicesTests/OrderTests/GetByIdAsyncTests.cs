using CustomCADSolutions.Core.Models;

namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    public class GetByIdAsyncTests : BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public void Test_DoesNotThrowWhenOrderExists(int id)
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await service.GetByIdAsync(id);
            }, "Couldn't find existing Order.");
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsCorrectly(int id)
        {
            OrderModel expectedOrder = orders.First(o => o.Id == id);
            OrderModel actualOrder = await service.GetByIdAsync(id);
            
            Assert.Multiple(() =>
            {
                Assert.That(actualOrder.Id, Is.EqualTo(expectedOrder.Id),
                    "Order Id mismatch.");

                Assert.That(actualOrder.Name, Is.EqualTo(expectedOrder.Name),
                    "Order Name mismatch.");

                Assert.That(actualOrder.Description, Is.EqualTo(expectedOrder.Description),
                    "Order Description mismatch.");

                Assert.That(actualOrder.OrderDate, Is.EqualTo(expectedOrder.OrderDate),
                    "Order OrderDate mismatch.");

                Assert.That(actualOrder.ShouldShow, Is.EqualTo(expectedOrder.ShouldShow),
                    "Order ShouldShow mismatch.");

                Assert.That(actualOrder.Status, Is.EqualTo(expectedOrder.Status),
                    "Order Status mismatch.");

                Assert.That(actualOrder.CategoryId, Is.EqualTo(expectedOrder.CategoryId),
                    "Order CategoryId mismatch.");

                Assert.That(actualOrder.CadId, Is.EqualTo(expectedOrder.CadId),
                    "Order CadId mismatch.");

                Assert.That(actualOrder.BuyerId, Is.EqualTo(expectedOrder.BuyerId),
                    "Order BuyerId mismatch.");

            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id);
            }, "Finds non-existent Order.");
        }
    }
}
