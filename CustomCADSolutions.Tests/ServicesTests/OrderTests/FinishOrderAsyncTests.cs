using static CustomCADSolutions.Core.TestsErrorMessages;
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

            Assert.That(finishedOrder.Status, Is.EqualTo(OrderStatus.Finished),
                OrderNotFinished);
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
                    string.Format(ShoultHaveBeenSaved, "Name"));

                Assert.That(actualCad.Bytes, Is.EqualTo(expectedCad.Bytes),
                    string.Format(ShoultHaveBeenSaved, "Bytes"));

                Assert.That(actualCad.CreatorId, Is.EqualTo(expectedCad.CreatorId),
                    string.Format(ShoultHaveBeenSaved, "CreatorId"));

                Assert.That(actualCad.Price, Is.EqualTo(expectedCad.Price),
                    string.Format(ShoultHaveBeenSaved, "Price"));

                Assert.That(actualCad.CategoryId, Is.EqualTo(expectedCad.CategoryId),
                    string.Format(ShoultHaveBeenSaved, "CategoryId"));

                Assert.That(actualCad.IsValidated, Is.EqualTo(expectedCad.IsValidated),
                    string.Format(ShoultHaveBeenSaved, "IsValidated"));

                Assert.That(actualCad.CreationDate, Is.EqualTo(expectedCad.CreationDate),
                    string.Format(ShoultHaveBeenSaved, "CreationDate"));

            });
        }
    }
}