using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class GetAllAsyncTests : BaseCategoriesTests
    {
        [Test]
        public async Task Test_ReturnsCorrectly()
        {
            var categories = await service.GetAllAsync().ConfigureAwait(false);

            Assert.That(categories.Count(), Is.EqualTo(this.categories.Count()),
                string.Format(ModelsCountMismatch, "Categories"));
        }
    }
}
