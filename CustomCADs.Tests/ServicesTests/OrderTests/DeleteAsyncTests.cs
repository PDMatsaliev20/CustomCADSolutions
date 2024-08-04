using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.OrderTests
{
    public class DeleteAsyncTests :  BaseOrdersTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DeletesWhenOrderExists(int id)
        {
            await service.DeleteAsync(id).ConfigureAwait(false);

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(DidNotDelete, "Order"));
        }
        
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenOrderDoesNotExists(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.DeleteAsync(id).ConfigureAwait(false);
            }, string.Format(ShouldNotHaveDeleted, "Order"));
        }
    }
}
