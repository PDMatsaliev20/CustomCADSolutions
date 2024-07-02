using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServicesTests.OrderTests
{
    public class DeleteAsyncTests :  BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DeletesWhenOrderExists(int id)
        {
            await service.DeleteAsync(id);

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id);
            }, string.Format(DidNotDelete, "Order"));
        }
        
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenOrderDoesNotExists(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.DeleteAsync(id);
            }, string.Format(ShouldNotHaveDeleted, "Order"));
        }
    }
}
