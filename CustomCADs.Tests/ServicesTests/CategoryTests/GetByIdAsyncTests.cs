using CustomCADs.Application.Models;
using static CustomCADs.Application.TestsErrorMessages;

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
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(ExistsButCannotFind, "Category"));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsCorrectly(int id)
        {
            CategoryModel expectedCategory = categories.First(c => c.Id == id);
            CategoryModel actualCategory = await service.GetByIdAsync(id).ConfigureAwait(false);

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
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(FindsButDoesNotExist, "Category"));
        }
    }
}
