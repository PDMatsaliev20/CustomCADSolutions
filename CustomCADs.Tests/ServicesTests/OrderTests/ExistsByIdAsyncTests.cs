using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    public class ExistsByIdAsyncTests : BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsTrueWhenOrderExists(int id)
        {
            bool orderExists = await service.ExistsByIdAsync(id).ConfigureAwait(false);
            Assert.That(orderExists, Is.True, 
                string.Format(DidNotFind, "Order"));
        }
        
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public async Task Test_ReturnsFalseWhenOrderDoesNotExists(int id)
        {
            bool exists = await service.ExistsByIdAsync(id).ConfigureAwait(false);
            Assert.That(exists, Is.False, 
                string.Format(ShouldNotHaveFound, "Order"));
        }
    }
}
