using CustomCADs.Core.Models.Cads;
using CustomCADs.Domain.Entities.Enums;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class EditAsyncTests : BaseCadsTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsDesiredProperties(int id)
        {
            CadModel expectedCad = await service.GetByIdAsync(id).ConfigureAwait(false);
            expectedCad.Name = "EditedCad";
            expectedCad.Price++;
            expectedCad.CategoryId = 2;
            expectedCad.Coords = [100, 100, 100];
            expectedCad.PanCoords = [200, 200, 200];

            await service.EditAsync(id, expectedCad).ConfigureAwait(false);
            CadModel actualCad = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name ),
                    string.Format(DoesNotEditEnough, "Name"));

                Assert.That(actualCad.Price, Is.EqualTo(expectedCad.Price),
                    string.Format(DoesNotEditEnough, "Price"));
                
                Assert.That(actualCad.CategoryId, Is.EqualTo(expectedCad.CategoryId ),
                    string.Format(DoesNotEditEnough, "CategoryId"));
                
                Assert.That(actualCad.Coords, Is.EqualTo(expectedCad.Coords),
                    string.Format(DoesNotEditEnough, "Coords"));
            });
        }

        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DoesNotEditUndesiredProperties(int id)
        {
            CadModel expectedCad = await service.GetByIdAsync(id).ConfigureAwait(false);
            expectedCad.Id = 100;
            expectedCad.CreationDate = DateTime.Now.AddDays(1);
            expectedCad.CreatorId = users[2].Id;
            expectedCad.Status = CadStatus.Banned;

            await service.EditAsync(id, expectedCad).ConfigureAwait(false);
            CadModel actualCad = await service.GetByIdAsync(id).ConfigureAwait(false);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Id, Is.Not.EqualTo(expectedCad.Id),
                    string.Format(EditsTooMuch, "Id"));

                Assert.That(actualCad.Status, Is.Not.EqualTo(expectedCad.Status),
                    string.Format(EditsTooMuch, "Status"));
                
                Assert.That(actualCad.CreationDate, Is.Not.EqualTo(expectedCad.CreationDate),
                    string.Format(EditsTooMuch, "CreationDate"));

                Assert.That(actualCad.CreatorId, Is.Not.EqualTo(expectedCad.CreatorId),
                    string.Format(EditsTooMuch, "CreatorId"));
            });
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenCadDoesNotExist(int id) 
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.EditAsync(id, new()).ConfigureAwait(false);
            }, string.Format(EditsNonExistent, "Cad"));
        }
    }
}
