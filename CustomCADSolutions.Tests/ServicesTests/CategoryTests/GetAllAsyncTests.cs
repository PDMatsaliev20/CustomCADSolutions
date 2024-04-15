using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Tests.ServicesTests.CategoryTests;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    [TestFixture]
    public class GetAllAsyncTests : BaseTests
    {
        [Test]
        public async Task Test_ReturnsProperly()
        {
            Category[] categories = (await service.GetAllAsync()).ToArray();
            var actualCategories = categories.OrderBy(c => c.Id);

            Assert.That(actualCategories, Is.EqualTo(this.categories), "Category mismatch.");
        }
    }
}
