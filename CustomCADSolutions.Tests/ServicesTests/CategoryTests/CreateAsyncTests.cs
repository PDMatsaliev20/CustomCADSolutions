using CustomCADSolutions.Core.Models;
using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
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
