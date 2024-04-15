using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Tests.ServicesTests.CategoryTests;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class CreateAsyncTests : BaseTests
    {
        [Test]
        [TestCase("newCategory")]
        public async Task Test_AddsAndReturnsIdProperly(string name)
        {
            Category newCategory = new() { Name = name };

            int id = await service.CreateAsync(newCategory);
            Category createdCategory = await service.GetByIdAsync(id);

            Assert.That(createdCategory.Name, Is.EqualTo(name), "Category mismatch.");
        }

        [Test]
        [TestCase(null)]
        public void Test_ThrowsProperly(string name)
        {
            Category newCategory = null!;

            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(newCategory);
            });
        }

    }
}
