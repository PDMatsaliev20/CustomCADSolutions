using CustomCADSolutions.Infrastructure.Data.Models;
using CustomCADSolutions.Tests.ServicesTests.CategoryTests;

namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    [TestFixture]
    public class GetByIdAsync : BaseTests
    {
        [TestCase(1)]
        [TestCase(5)]
        public async Task Test_ReturnsProperly(int id)
        {
            Category expectedCategory = categories.ToList().Find(c => c.Id == id)!;
            Category? actualCategory = await service.GetByIdAsync(id);

            Assert.That(actualCategory, Is.Not.Null, "Missing Category.");
            Assert.That(actualCategory, Is.EqualTo(expectedCategory), "Category mismatch.");
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public async Task Test_ThrowsProperly(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                Category? category = await service.GetByIdAsync(id);
            }, "Non-existing Category.");
        }
    }
}
