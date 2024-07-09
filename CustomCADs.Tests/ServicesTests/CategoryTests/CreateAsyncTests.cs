using CustomCADs.Core.Models;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class CreateAsyncTests : BaseCategoriesTests
    {
        [Test]
        public async Task Test_AddsCorrectly()
        {
            CategoryModel expectedCategory = new() { Name = "NewCategory" };

            int id = await service.CreateAsync(expectedCategory);
            CategoryModel actualCategory = await service.GetByIdAsync(id);

            Assert.That(expectedCategory.Name, Is.EqualTo(actualCategory.Name),
                string.Format(ModelPropertyMismatch, "Name"));
        }

        [Test]
        public void Test_ThrowsWhenCategoryIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(null!);
            }, string.Format(AddedNull, "Category"));
        }
    }
}
