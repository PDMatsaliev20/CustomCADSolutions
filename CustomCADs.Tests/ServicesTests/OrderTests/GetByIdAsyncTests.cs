using CustomCADs.Core.Models;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
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
            }, string.Format(ExistsButCannotFind, "Order"));
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
                    string.Format(ModelPropertyMismatch, "Id"));

                Assert.That(actualOrder.Name, Is.EqualTo(expectedOrder.Name),
                    string.Format(ModelPropertyMismatch, "Name"));

                Assert.That(actualOrder.Description, Is.EqualTo(expectedOrder.Description),
                    string.Format(ModelPropertyMismatch, "Description"));

                Assert.That(actualOrder.OrderDate, Is.EqualTo(expectedOrder.OrderDate),
                    string.Format(ModelPropertyMismatch, "OrderDate"));

                Assert.That(actualOrder.ShouldBeDelivered, Is.EqualTo(expectedOrder.ShouldBeDelivered),
                    string.Format(ModelPropertyMismatch, "ShouldShow"));

                Assert.That(actualOrder.Status, Is.EqualTo(expectedOrder.Status),
                    string.Format(ModelPropertyMismatch, "Status"));

                Assert.That(actualOrder.CategoryId, Is.EqualTo(expectedOrder.CategoryId),
                    string.Format(ModelPropertyMismatch, "CategoryId"));

                Assert.That(actualOrder.CadId, Is.EqualTo(expectedOrder.CadId),
                    string.Format(ModelPropertyMismatch, "CadId"));

                Assert.That(actualOrder.BuyerId, Is.EqualTo(expectedOrder.BuyerId),
                    string.Format(ModelPropertyMismatch, "BuyerId"));

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
            }, string.Format(FindsButDoesNotExist, "Order"));
        }
    }
}
