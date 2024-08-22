using CustomCADs.Application.Models.Cads;
using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
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
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(ExistsButCannotFind, "Cads"));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public void Test_DoesNotThrowWhenCadExists(int id)
        {
            Assert.DoesNotThrowAsync(async () =>
            {
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(FindsButDoesNotExist, "Cad"));
        }
        
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ReturnsCorrectly(int id)
        {
            CadModel expectedCad = this.cads.First(cad => cad.Id == id);
            CadModel actualCad = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Id, Is.EqualTo(expectedCad.Id),
                    string.Format(ModelPropertyMismatch, "Id"));

                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name),
                    string.Format(ModelPropertyMismatch, "Name"));

                Assert.That(actualCad.Paths.FileExtension, Is.EqualTo(expectedCad.Paths.FileExtension),
                    string.Format(ModelPropertyMismatch, "Cad"));
                
                Assert.That(actualCad.Paths.ImageExtension, Is.EqualTo(expectedCad.Paths.ImageExtension),
                    string.Format(ModelPropertyMismatch, "Image"));

                Assert.That(actualCad.Status, Is.EqualTo(expectedCad.Status),
                    string.Format(ModelPropertyMismatch, "Status"));

                Assert.That(actualCad.CategoryId, Is.EqualTo(expectedCad.CategoryId),
                    string.Format(ModelPropertyMismatch, "CategoryId"));

                Assert.That(actualCad.CamCoordinates, Is.EqualTo(expectedCad.CamCoordinates),
                    string.Format(ModelPropertyMismatch, "Cam Coordinates"));
                
                Assert.That(actualCad.PanCoordinates, Is.EqualTo(expectedCad.PanCoordinates),
                    string.Format(ModelPropertyMismatch, "Pan Coordinates"));

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
