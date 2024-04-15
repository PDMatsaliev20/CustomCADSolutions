using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Tests.ServicesTests.CategoryTests;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class CreateAsyncTests : BaseTests
    {
        [Test]
        [TestCase("newCategory")]
        public void Test_AddsProperly(string name)
        {
            Category newCategory = new() { Name = name };

            Assert.DoesNotThrowAsync(async () =>
            {
                await service.CreateAsync(newCategory);
            }, "Adding Category failed.");
        }
        
        [Test]
        [TestCase("newCategory")]
        public async Task Test_ReturnsIdProperly(string name)
        {
            Category newCategory = new() { Name = name };

            int id = await service.CreateAsync(newCategory);
            Category createdCategory = await service.GetByIdAsync(id);

            Assert.That(createdCategory.Name, Is.EqualTo(name), "Category mismatch.");
        }
        
        [Test]
        [TestCase("newCategoryButIt'sDefinitelyWayTooLong")]
        public void Test_ThrowsProperlyWhenNameTooLong(string name)
        {
            Category newCategory = new() { Name = name };

            Assert.DoesNotThrowAsync(async () =>
            {
                await service.CreateAsync(newCategory);
            });
        }

        [Test]
        public void Test_ThrowsProperlyWhenNullCategory()
        {
            Category newCategory = null!;

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(newCategory);
            });
        }
    }
}
