﻿using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CategoryTests
{
    public class DeleteAsyncTests : BaseCategoriesTests
    {
        [TestCase(1)]
        [TestCase(4)]
        public async Task Test_DeletesWhenCategoryExists(int id)
        {
            await service.DeleteAsync(id).ConfigureAwait(false);

            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.GetByIdAsync(id).ConfigureAwait(false);
            }, string.Format(DidNotDelete, "Category"));
        }

        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(100)]
        public void Test_ThrowsWhenCategoryDoesNotExist(int id)
        {
            Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            {
                await service.DeleteAsync(id).ConfigureAwait(false);
            }, string.Format(ShouldNotHaveDeleted, "Category"));
        }
    }
}
