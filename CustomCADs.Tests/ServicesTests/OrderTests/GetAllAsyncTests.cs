using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    public class GetAllAsyncTests : BaseOrdersTests
    {
        [Test]
        public async Task Test_ReturnsCorrectly()
        {
            var result = await service.GetAllAsync(new(), new(), new()).ConfigureAwait(false);

            Assert.That(result.Count, Is.EqualTo(orders.Length),
                string.Format(ModelsCountMismatch, "Orders"));
        }
    }
}
