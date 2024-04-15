using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    public class FinishOrderAsyncTests : BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_OrderStatusIsFinished(int id)
        {
            OrderModel order = await service.GetByIdAsync(id);
            order.Cad = new() 
            {
                Name = "New Model",
                Bytes = new byte[1],
                CreatorId = users[1].Id,
            };

            await service.FinishOrderAsync(id, order);
            OrderModel finishedOrder = await service.GetByIdAsync(id);

            Assert.That(finishedOrder.Status, Is.EqualTo(OrderStatus.Finished));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_CadInfoIsNotLost(int id)
        {
            OrderModel order = await service.GetByIdAsync(id);
            order.Cad = new() 
            {
                Name = "New Model",
                Bytes = new byte[1],
                CreatorId = users[1].Id,
                Price = 50M,
                CategoryId = 3,
                IsValidated = true,
                CreationDate = DateTime.Now,
            };

            await service.FinishOrderAsync(id, order);
            OrderModel finishedOrder = await service.GetByIdAsync(id);

            CadModel expectedCad = order.Cad;
            CadModel? actualCad = finishedOrder.Cad;

            Assert.That(actualCad, Is.Not.Null);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name),
                    "Cad's Name does not get saved.");

                Assert.That(actualCad.Bytes, Is.EqualTo(expectedCad.Bytes), 
                    "Cad's Bytes does not get saved.");

                Assert.That(actualCad.CreatorId, Is.EqualTo(expectedCad.CreatorId), 
                    "Cad's CreatorId does not get saved.");

                Assert.That(actualCad.Price, Is.EqualTo(expectedCad.Price), 
                    "Cad's Price does not get saved.");

                Assert.That(actualCad.CategoryId, Is.EqualTo(expectedCad.CategoryId), 
                    "Cad's CategoryId does not get saved.");

                Assert.That(actualCad.IsValidated, Is.EqualTo(expectedCad.IsValidated), 
                    "Cad's IsValidated does not get saved.");

                Assert.That(actualCad.CreationDate, Is.EqualTo(expectedCad.CreationDate), 
                    "Cad's CreationDate does not get saved.");

            });
        }
    }
}