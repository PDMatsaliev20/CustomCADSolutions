namespace CustomCADSolutions.Tests.ServiceTests.CategoryTests
{
    public class GetAllAsyncTests : BaseCategoriesTests
    {
        [Test]
        public async Task Test_ReturnsCorrectly()
        {
            var categories = await service.GetAllAsync();

            Assert.That(categories.Count(), Is.EqualTo(this.categories.Count()), 
                "Categories Count mismatch.");
        }
    }
}
