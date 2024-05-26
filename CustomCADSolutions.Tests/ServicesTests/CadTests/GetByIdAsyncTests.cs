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

                Assert.That(actualCad.Product.Name, Is.EqualTo(expectedCad.Product.Name),
                    string.Format(ModelPropertyMismatch, "Name"));

                Assert.That(actualCad.Bytes, Is.EqualTo(expectedCad.Bytes),
                    string.Format(ModelPropertyMismatch, "Bytes"));

                Assert.That(actualCad.Product.IsValidated, Is.EqualTo(expectedCad.Product.IsValidated),
                    string.Format(ModelPropertyMismatch, "IsValidated"));

                Assert.That(actualCad.Product.CategoryId, Is.EqualTo(expectedCad.Product.CategoryId),
                    string.Format(ModelPropertyMismatch, "CategoryId"));

                Assert.That(actualCad.Coords, Is.EqualTo(expectedCad.Coords),
                    string.Format(ModelPropertyMismatch, "Coords"));

                Assert.That(actualCad.CreatorId, Is.EqualTo(expectedCad.CreatorId),
                    string.Format(ModelPropertyMismatch, "CreatorId"));

                Assert.That(actualCad.CreationDate, Is.EqualTo(expectedCad.CreationDate),
                    string.Format(ModelPropertyMismatch, "CreationDate"));

                Assert.That(actualCad.Product.Price, Is.EqualTo(expectedCad.Product.Price),
                    string.Format(ModelPropertyMismatch, "Price"));
            });
        }
    }
}
