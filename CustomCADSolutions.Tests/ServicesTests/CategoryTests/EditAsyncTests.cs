using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Tests.ServicesTests.CategoryTests;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    [TestFixture]
    public class EditAsyncTests : BaseTests
    {
        [Test]
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsProperly(int id)
        {
            Category category = new()
            {
                Id = id,
                Name = "EditedCategory",
            };

            await service.EditAsync(id, category);
            Category editedCategory = await service.GetByIdAsync(1);

            Assert.That(editedCategory.Name, Is.EqualTo(category.Name), "Edited Category doesn't get saved.");
        }

        [Test]
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsProperly(int id)
        {
            Category category = new()
            {
                Id = id,
                Name = "EditedCategory",
            };

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.EditAsync(id, category);
            }, "Non-existent category gets edited");
        }
    }
}
