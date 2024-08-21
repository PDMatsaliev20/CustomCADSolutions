using CustomCADs.Application.Models.Orders;
using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
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
            int id = await service.CreateAsync(expected).ConfigureAwait(false);

            OrderModel actual = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(actual.Name, Is.EqualTo(expected.Name),  
                    string.Format(ModelPropertyMismatch, "Name"));

                Assert.That(actual.Description, Is.EqualTo(expected.Description),  
                    string.Format(ModelPropertyMismatch, "Description"));
                
                Assert.That(actual.CategoryId, Is.EqualTo(expected.CategoryId),  
                    string.Format(ModelPropertyMismatch, "CategoryId"));
                
                Assert.That(actual.BuyerId, Is.EqualTo(expected.BuyerId), 
                    string.Format(ModelPropertyMismatch, "BuyerId"));
            });
        }

        [Test]
        public void Test_ThrowsWhenOrderIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(null!).ConfigureAwait(false);
            }, string.Format(AddedNull, "Category"));
        }
    }
}
