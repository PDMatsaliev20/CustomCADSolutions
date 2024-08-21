﻿using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class DeleteAsyncTests : BaseCadsTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DeletesWhenCadExists(int id)
        {
            await service.DeleteAsync(id).ConfigureAwait(false);

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(DidNotDelete, "Cad"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenCadDoesNotExists(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.DeleteAsync(id).ConfigureAwait(false);
            }, string.Format(ShouldNotHaveDeleted, "Cad"));
        }
    }
}
