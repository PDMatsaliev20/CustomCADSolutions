using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class CountTests : BaseCadsTests
    {
        [Test]
        public async Task Test_CountsCorrectly()
        {
            int expectedNoneCount = 0;
            int actualNoneCount = await service.Count(c => string.IsNullOrEmpty(c.Name));
            
            int expectedAllCount = cads.Length;
            int actualAllCount = await service.Count(c => !string.IsNullOrEmpty(c.Name));

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
