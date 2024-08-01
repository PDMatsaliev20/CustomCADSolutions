using CustomCADs.Core.Models.Cads;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class CreateAsyncTests : BaseCadsTests
    {
        [Test]
        public async Task Test_AddsCorrectly()
        {
            CadModel expectedCad = new() { Id = 10, Name = "NewCad", CreatorId = users[2].Id, CadExtension = ".gltf", ImageExtension = ".png" };

            int id = await service.CreateAsync(expectedCad);
            CadModel actualCad = await service.GetByIdAsync(id);

            Assert.Multiple(() =>
            {
                Assert.That(actualCad.Id, Is.EqualTo(expectedCad.Id),
                    string.Format(ModelPropertyMismatch, "Id"));

                Assert.That(actualCad.Name, Is.EqualTo(expectedCad.Name),
                    string.Format(ModelPropertyMismatch, "Name"));

                Assert.That(actualCad.CreatorId, Is.EqualTo(expectedCad.CreatorId),
                    string.Format(ModelPropertyMismatch, "CreatorId"));
            });
        }

        [Test]
        public void Test_ThrowsWhenCadIsNull()
        {
            Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await service.CreateAsync(null!);
            }, string.Format(AddedNull, "Cad"));
        }
    }
}
