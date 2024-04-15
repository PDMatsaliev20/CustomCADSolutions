using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class GetByIdAsyncTests : BaseCategoriesTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public void Test_DoesNotThrowWhenCategoryExists(int id)
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await service.GetByIdAsync(id);
            }, "Couldn't find existing Category.");
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsCorrectly(int id)
        {
            Category expectedCategory = categories.First(c => c.Id == id);
            Category actualCategory = await service.GetByIdAsync(id);

            Assert.That(actualCategory.Name, Is.EqualTo(expectedCategory.Name), "Category Name mismatch.");
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id);
            }, "Finds non-existent Category.");
        }
    }
}
