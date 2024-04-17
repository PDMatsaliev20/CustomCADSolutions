using CustomCADSolutions.Core.Models;
using System.Drawing;
using static CustomCADSolutions.Core.TestsErrorMessages;

namespace CustomCADSolutions.Tests.ServicesTests.CadTests
{
    public class ChangeColorAsyncTests : BaseCadsTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_ColorSavedCorrectly(int id)
        {
            Color expectedColor = Color.FromArgb(20, 20, 20);
            
            await service.ChangeColorAsync(id, expectedColor);
            CadModel cad = await service.GetByIdAsync(id);
            Color actualColor = cad.Color;

            Assert.Multiple(() =>
            {
                Assert.That(actualColor.R, Is.EqualTo(expectedColor.R),
                    message: string.Format(ModelPropertyMismatch, "R"));

                Assert.That(actualColor.G, Is.EqualTo(expectedColor.G),
                    message: string.Format(ModelPropertyMismatch, "G"));

                Assert.That(actualColor.B, Is.EqualTo(expectedColor.B),
                    message: string.Format(ModelPropertyMismatch, "B"));

            });
        }
    }
}
