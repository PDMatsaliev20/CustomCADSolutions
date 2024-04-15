using CustomCADSolutions.Infrastructure.Data.Models;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class CreateAsyncTests : BaseCategoriesTests
    {
        [Test]
        public async Task Test_AddsCorrectly()
        {
            Category expectedCategory = new() { Name = "NewCategory" };

            int id = await service.CreateAsync(expectedCategory);
            Category actualCategory = await service.GetByIdAsync(id);

            Assert.That(expectedCategory.Name, Is.EqualTo(actualCategory.Name));
        }

        [Test]
        public void Test_ThrowsProperlyWhenNullCategory()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(null!);
            }, "Added null Category.");
        }
    }
}
