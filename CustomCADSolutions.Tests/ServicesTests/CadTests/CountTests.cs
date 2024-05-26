using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServicesTests.CadTests
{
    public class CountTests : BaseCadsTests
    {
        [Test]
        public void Test_CountsCorrectly()
        {
            int expectedNoneCount = 0;
            int actualNoneCount = service.Count(c => string.IsNullOrEmpty(c.Product.Name));
            
            int expectedAllCount = cads.Length;
            int actualAllCount = service.Count(c => !string.IsNullOrEmpty(c.Product.Name));

            Assert.Multiple(() =>
            {
                Assert.That(actualNoneCount, Is.EqualTo(expectedNoneCount),
                    CountedWrong);

                Assert.That(actualAllCount, Is.EqualTo(expectedAllCount),
                    CountedWrong);
            });
        }
    }
}
