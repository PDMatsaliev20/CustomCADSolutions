using CustomCADs.Core.Models;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
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
            }, string.Format(ExistsButCannotFind, "Category"));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsCorrectly(int id)
        {
            CategoryModel expectedCategory = categories.First(c => c.Id == id);
            CategoryModel actualCategory = await service.GetByIdAsync(id);

            Assert.That(actualCategory.Name, Is.EqualTo(expectedCategory.Name), 
                string.Format(ModelPropertyMismatch, "Name"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id);
            }, string.Format(FindsButDoesNotExist, "Category"));
        }
    }
}
