﻿using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    public  class EditAsyncTests : BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsDesiredProperties(int id)
        {
            OrderModel expectedOrder = await service.GetByIdAsync(id).ConfigureAwait(false);
            expectedOrder.Name = "EditedOrder";
            expectedOrder.Description = "Client's Edited Order";
            expectedOrder.CategoryId = 2;
            expectedOrder.Status = OrderStatus.Finished;
            expectedOrder.ShouldBeDelivered = false;

            await service.EditAsync(id, expectedOrder).ConfigureAwait(false);
            OrderModel actualOrder = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(actualOrder.Name, Is.EqualTo(expectedOrder.Name),
                    string.Format(DoesNotEditEnough, "Name"));

                Assert.That(actualOrder.Description, Is.EqualTo(expectedOrder.Description),
                    string.Format(DoesNotEditEnough, "Description"));

                Assert.That(actualOrder.CategoryId, Is.EqualTo(expectedOrder.CategoryId),
                    string.Format(DoesNotEditEnough, "CategoryId"));

                Assert.That(actualOrder.ShouldBeDelivered, Is.EqualTo(expectedOrder.ShouldBeDelivered),
                    string.Format(DoesNotEditEnough, "ShouldShow"));

            });
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DoesNotEditUndesiredProperties(int id)
        {
            OrderModel expectedOrder = await service.GetByIdAsync(id).ConfigureAwait(false);
            expectedOrder.Id = 1000;
            expectedOrder.OrderDate = DateTime.Now.AddYears(-500);
            expectedOrder.BuyerId = users[2].Id;

            await service.EditAsync(id, expectedOrder).ConfigureAwait(false);
            OrderModel actualOrder = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(actualOrder.Id, Is.Not.EqualTo(expectedOrder.Id),
                    string.Format(EditsTooMuch, "Id"));

                Assert.That(actualOrder.BuyerId, Is.Not.EqualTo(expectedOrder.BuyerId),
                    string.Format(EditsTooMuch, "BuyerId"));

                Assert.That(actualOrder.OrderDate, Is.Not.EqualTo(expectedOrder.OrderDate),
                    string.Format(EditsTooMuch, "OrderDate"));
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenOrderDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.EditAsync(id, new()).ConfigureAwait(false);
            }, string.Format(EditsNonExistent, "Order"));
        }
    }
}
