using CustomCADSolutions.Core.Models;
using System.Drawing;
using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServicesTests.CadTests
{
    public class EditAsyncTests : BaseCadsTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_EditsDesiredProperties(int id)
        {
            CadModel expectedCad = await service.GetByIdAsync(id);
            expectedCad.Name = "EditedCad";
            expectedCad.IsValidated = !expectedCad.IsValidated;
            expectedCad.Price++;
            expectedCad.CategoryId = 2;
            expectedCad.SpinAxis = null;
            expectedCad.Coords = new int[3] { 100, 100, 100 };

            await service.EditAsync(id, expectedCad);
            CadModel actualCad = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name ),
                    string.Format(DoesNotEditEnough, "Name"));

                Assert.That(actualCad.IsValidated, Is.EqualTo(expectedCad.IsValidated ),
                    string.Format(DoesNotEditEnough, "IsValidated"));
                
                Assert.That(actualCad.Price, Is.EqualTo(expectedCad.Price),
                    string.Format(DoesNotEditEnough, "Price"));
                
                Assert.That(actualCad.CategoryId, Is.EqualTo(expectedCad.CategoryId ),
                    string.Format(DoesNotEditEnough, "CategoryId"));
                
                Assert.That(actualCad.SpinAxis, Is.EqualTo(expectedCad.SpinAxis ),
                    string.Format(DoesNotEditEnough, "SpinAxis"));
                
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
            expectedCad.Bytes = new byte[100];
            expectedCad.Color = Color.FromArgb(200, 200, 200);
            expectedCad.CreationDate = DateTime.Now.AddDays(1);
            expectedCad.CreatorId = users[2].Id;

            await service.EditAsync(id, expectedCad);
            CadModel actualCad = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Id, Is.Not.EqualTo(expectedCad.Id),
                    string.Format(EditsTooMuch, "Id"));

                Assert.That(actualCad.Bytes, Is.Not.EqualTo(expectedCad.Bytes),
                    string.Format(EditsTooMuch, "Bytes"));

                Assert.That(actualCad.Color, Is.Not.EqualTo(expectedCad.Color),
                    string.Format(EditsTooMuch, "Color"));

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
