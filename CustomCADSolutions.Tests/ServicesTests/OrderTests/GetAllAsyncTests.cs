namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    public class GetAllAsyncTests : BaseOrdersTests
    {
        [Test]
        public async Task Test_ReturnsCorrectly()
        {
            var orders = await service.GetAllAsync();

            Assert.That(orders.Count(), Is.EqualTo(this.orders.Count()),
                "Orders Count mismatch.");
        }
    }
}
