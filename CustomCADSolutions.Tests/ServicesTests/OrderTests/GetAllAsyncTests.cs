using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServicesTests.OrderTests
{
    public class GetAllAsyncTests : BaseOrdersTests
    {
        [Test]
        public async Task Test_ReturnsCorrectly()
        {
            var orders = await service.GetAllAsync();

            Assert.That(orders.Count(), Is.EqualTo(this.orders.Count()),
                string.Format(ModelsCountMismatch, "Orders"));
        }
    }
}
