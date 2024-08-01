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
            CadModel expectedCad = await service.GetByIdAsync(id);
            expectedCad.Name = "EditedCad";
            expectedCad.Status = CadStatus.Banned;
            expectedCad.Price++;
            expectedCad.CategoryId = 2;
            expectedCad.Coords = [100, 100, 100];
            expectedCad.PanCoords = [200, 200, 200];

            await service.EditAsync(id, expectedCad);
            CadModel actualCad = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name ),
                    string.Format(DoesNotEditEnough, "Name"));

                Assert.That(actualCad.Status, Is.EqualTo(expectedCad.Status),
                    string.Format(DoesNotEditEnough, "Status"));
                
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
            CadModel expectedCad = await service.GetByIdAsync(id);
            expectedCad.Id = 100;
            expectedCad.CadExtension = "abc";
            expectedCad.ImageExtension = "xyz";
            expectedCad.CreationDate = DateTime.Now.AddDays(1);
            expectedCad.CreatorId = users[2].Id;

            await service.EditAsync(id, expectedCad);
            CadModel actualCad = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Id, Is.Not.EqualTo(expectedCad.Id),
                    string.Format(EditsTooMuch, "Id"));

                Assert.That(actualCad.CadExtension, Is.Not.EqualTo(expectedCad.CadExtension),
                    string.Format(EditsTooMuch, "Cad"));
                
                Assert.That(actualCad.ImageExtension, Is.Not.EqualTo(expectedCad.ImageExtension),
                    string.Format(EditsTooMuch, "Image"));

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
                await service.EditAsync(id, new());
            }, string.Format(EditsNonExistent, "Cad"));
        }
    }
}
