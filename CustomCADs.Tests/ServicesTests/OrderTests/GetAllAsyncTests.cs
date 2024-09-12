using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    public class GetAllAsyncTests : BaseOrdersTests
    {
        [Test]
        public void Test_ReturnsCorrectly()
        {
            var result = service.GetAll();

            Assert.That(result.Count, Is.EqualTo(orders.Length),
                string.Format(ModelsCountMismatch, "Orders"));
        }
    }
}
