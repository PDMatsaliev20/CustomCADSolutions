namespace CustomCADSolutions.Tests.ServiceTests.OrderTests
{
    public class ExistsByIdAsyncTests : BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsTrueWhenOrderExists(int id)
        {
            bool orderExists = await service.ExistsByIdAsync(id);
            Assert.That(orderExists, Is.True, "Didn't find existing Order.");
        }
        
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public async Task Test_ReturnsFalseWhenOrderDoesNotExists(int id)
        {
            bool exists = await service.ExistsByIdAsync(id);
            Assert.That(exists, Is.False, "Found non-existent Order.");
        }
    }
}
