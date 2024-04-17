using CustomCADSolutions.Core.Models;
using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServicesTests.CadTests
{
    public class GetByIdAsyncTests : BaseCadsTests
    {
        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenCadDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id);
            }, string.Format(ExistsButCannotFind, "Cads"));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public void Test_DoesNotThrowWhenCadExists(int id)
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await service.GetByIdAsync(id);
            }, string.Format(FindsButDoesNotExist, "Cad"));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsCorrectly(int id)
        {
            CadModel expectedCad = this.cads.First(cad => cad.Id == id);
            CadModel actualCad = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Id, Is.EqualTo(expectedCad.Id),
                    string.Format(ModelPropertyMismatch, "Id"));

                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name),
                    string.Format(ModelPropertyMismatch, "Name"));

                Assert.That(actualCad.Bytes, Is.EqualTo(expectedCad.Bytes),
                    string.Format(ModelPropertyMismatch, "Bytes"));

                Assert.That(actualCad.SpinAxis, Is.EqualTo(expectedCad.SpinAxis),
                    string.Format(ModelPropertyMismatch, "SpinAxis"));

                Assert.That(actualCad.IsValidated, Is.EqualTo(expectedCad.IsValidated),
                    string.Format(ModelPropertyMismatch, "IsValidated"));

                Assert.That(actualCad.Color, Is.EqualTo(expectedCad.Color),
                    string.Format(ModelPropertyMismatch, "Color"));

                Assert.That(actualCad.CategoryId, Is.EqualTo(expectedCad.CategoryId),
                    string.Format(ModelPropertyMismatch, "CategoryId"));

                Assert.That(actualCad.Coords, Is.EqualTo(expectedCad.Coords),
                    string.Format(ModelPropertyMismatch, "Coords"));

                Assert.That(actualCad.CreatorId, Is.EqualTo(expectedCad.CreatorId),
                    string.Format(ModelPropertyMismatch, "CreatorId"));

                Assert.That(actualCad.CreationDate, Is.EqualTo(expectedCad.CreationDate),
                    string.Format(ModelPropertyMismatch, "CreationDate"));

                Assert.That(actualCad.Price, Is.EqualTo(expectedCad.Price),
                    string.Format(ModelPropertyMismatch, "Price"));
            });
        }
    }
}
