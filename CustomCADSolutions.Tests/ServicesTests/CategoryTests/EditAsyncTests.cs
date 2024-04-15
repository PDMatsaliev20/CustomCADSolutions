using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class EditAsyncTests : BaseCategoriesTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsDesiredProperties(int id)
        {
            Category expectedCategory = await service.GetByIdAsync(id);
            expectedCategory.Name = "Edited Category";

            await service.EditAsync(id, expectedCategory);
            Category actualCategory = await service.GetByIdAsync(id);

            Assert.That(actualCategory.Name, Is.EqualTo(expectedCategory.Name),
                "Category's Name doesn't get saved.");
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenCategoryDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.EditAsync(id, new());
            }, "Non-existent Category edited.");
        }
    }
}
