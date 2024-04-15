using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    public  class EditAsyncTests : BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsDesiredProperties(int id)
        {
            OrderModel expectedOrder = await service.GetByIdAsync(id);
            expectedOrder.Name = "EditedOrder";
            expectedOrder.Description = "Client's Edited Order";
            expectedOrder.CategoryId = 2;
            expectedOrder.Status = OrderStatus.Finished;
            expectedOrder.ShouldShow = false;

            await service.EditAsync(id, expectedOrder);
            OrderModel actualOrder = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualOrder.Name, Is.EqualTo(expectedOrder.Name), 
                    "Order's Name doesn't get saved.");

                Assert.That(actualOrder.Description, Is.EqualTo(expectedOrder.Description), 
                    "Order's Description doesn't get saved.");

                Assert.That(actualOrder.CategoryId, Is.EqualTo(expectedOrder.CategoryId), 
                    "Order's CategoryId doesn't get saved.");

                Assert.That(actualOrder.Status, Is.EqualTo(expectedOrder.Status), 
                    "Order's Status doesn't get saved.");

                Assert.That(actualOrder.ShouldShow, Is.EqualTo(expectedOrder.ShouldShow), 
                    "Order's ShouldShow doesn't get saved.");

            });
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DoesNotEditsUndesiredProperties(int id)
        {
            OrderModel expectedOrder = await service.GetByIdAsync(id);
            expectedOrder.Id = 1000;
            expectedOrder.OrderDate = DateTime.Now.AddYears(-500);
            expectedOrder.BuyerId = users[2].Id;

            await service.EditAsync(id, expectedOrder);
            OrderModel actualOrder = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualOrder.Id, Is.Not.EqualTo(expectedOrder.Id), "New Id gets saved.");
                Assert.That(actualOrder.BuyerId, Is.Not.EqualTo(expectedOrder.BuyerId), "New BuyerId gets saved.");
                Assert.That(actualOrder.OrderDate, Is.Not.EqualTo(expectedOrder.OrderDate), "New OrderDate gets saved.");
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenOrderDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.EditAsync(id, new());
            }, "Non-existent Order edited.");
        }
    }
}
