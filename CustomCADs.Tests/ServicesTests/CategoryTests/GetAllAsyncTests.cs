using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class GetAllAsyncTests : BaseCategoriesTests
    {
        [Test]
        public void Test_ReturnsCorrectly()
        {
            var categories = service.GetAll();

            Assert.That(categories.Count(), Is.EqualTo(this.categories.Count()),
                string.Format(ModelsCountMismatch, "Categories"));
        }
    }
}
