using CustomCADSolutions.Tests.ServicesTests.CategoryTests;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class DeleteAsyncTests : BaseTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DeletesProperly(int id)
        {
            await service.DeleteAsync(id);

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id);
            }, "Deleted category still exists.");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsProperly(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.DeleteAsync(id);
            }, "Non-existent category gets deleted.");
        }
    }
}
