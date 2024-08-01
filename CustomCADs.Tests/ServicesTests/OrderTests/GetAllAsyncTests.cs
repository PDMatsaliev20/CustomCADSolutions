using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    public class GetAllAsyncTests : BaseOrdersTests
    {
        [Test]
        public async Task Test_ReturnsCorrectly()
        {
            var result = await service.GetAllAsync(new(), new(), new());

            Assert.That(result.Count, Is.EqualTo(orders.Length),
                string.Format(ModelsCountMismatch, "Orders"));
        }
    }
}
