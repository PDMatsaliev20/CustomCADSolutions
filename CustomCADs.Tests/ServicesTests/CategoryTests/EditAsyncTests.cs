using CustomCADs.Core.Models;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class EditAsyncTests : BaseCategoriesTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsDesiredProperties(int id)
        {
            CategoryModel expectedCategory = await service.GetByIdAsync(id).ConfigureAwait(false);
            expectedCategory.Name = "Edited Category";

            await service.EditAsync(id, expectedCategory).ConfigureAwait(false);
            CategoryModel actualCategory = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.That(actualCategory.Name, Is.EqualTo(expectedCategory.Name),
                string.Format(DoesNotEditEnough, "Name"));
        }

        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DoesNotEditUndesiredProperties(int id)
        {
            CategoryModel expectedCategory = await service.GetByIdAsync(id).ConfigureAwait(false);
            
            expectedCategory.Id = 100;
            CategoryModel actualCategory = await service.GetByIdAsync(id).ConfigureAwait(false);
            
            Assert.That(actualCategory.Id, Is.Not.EqualTo(expectedCategory.Id),
                    string.Format(EditsTooMuch, "Id"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenCategoryDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.EditAsync(id, new()).ConfigureAwait(false);
            }, string.Format(EditsNonExistent, "Category"));
        }
    }
}
