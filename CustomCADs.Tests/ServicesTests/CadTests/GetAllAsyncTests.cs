using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Entities.Enums;
using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class GetAllAsyncTests : BaseCadsTests
    {
        [Test]
        public async Task Test_ReturnsAllWithNoFilters()
        {
            int expectedCount = this.cads.Length;
            
            CadResult result = await service.GetAllAsync(new(), new(), new()).ConfigureAwait(false);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "None"));
        }

        [TestCase("Category1")]
        [TestCase("Category4")]
        public async Task Test_ReturnsCorrectlyWithCategoryFilter(string categoryName)
        {
            int expectedCount = this.cads.Count(c => c.Category.Name == categoryName);
            SearchModel search = new() { Category = categoryName };

            CadResult result = await service.GetAllAsync(new(), search, new()).ConfigureAwait(false);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Category"));
        }

        [TestCase("Client")]
        [TestCase("Contributor")]
        public async Task Test_ReturnsCorrectlyWithCreatorFilter(string creatorName)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName == creatorName);
            SearchModel search = new() { Owner = creatorName };

            CadResult result = await service.GetAllAsync(new(), search, new()).ConfigureAwait(false);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Creator"));
        }

        [TestCase("Cad")]
        [TestCase("3")]
        public async Task Test_ReturnsCorrectlyWithLikeNameFilter(string likeName)
        {
            int expectedCount = this.cads.Count(c => c.Name.Contains(likeName));
            SearchModel search = new() { Name = likeName };

            CadResult result = await service.GetAllAsync(new(), search, new()).ConfigureAwait(false);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeName"));
        }

        [TestCase("C")]
        [TestCase("ient")]
        [TestCase("ontr")]
        public async Task Test_ReturnsCorrectlyWithLikeCreatorFilter(string likeCreator)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName!.Contains(likeCreator));
            SearchModel search = new() { Owner = likeCreator };

            CadResult result = await service.GetAllAsync(new(), search, new()).ConfigureAwait(false);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeCreator"));
        }

        [TestCase(CadStatus.Unchecked)]
        [TestCase(CadStatus.Validated)]
        [TestCase(CadStatus.Reported)]
        [TestCase(CadStatus.Banned)]
        [TestCase(null)]
        public async Task Test_ReturnsCorrectlyWithStatusFilters(CadStatus? status)
        {
            int expectedCount = 0;
            if (status != null)
            {
                expectedCount = cads.Count(c => c.Status == status);
            }
            else
            {
                expectedCount = cads.Length;
            }

            CadQuery query = new() { Status = status };

            CadResult result = await service.GetAllAsync(query, new(), new()).ConfigureAwait(false);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Status"));
        }

        [TestCase(1, 3)]
        [TestCase(2, 3)]
        [TestCase(3, 3)]
        [TestCase(1, 2)]
        [TestCase(4, 2)]
        public async Task Test_ReturnsCorrectlyWithPagination(int page, int limit)
        {
            int[] expectedCadIds = this.cads
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(c => c.Id)
                .ToArray();

            PaginationModel pagination = new() { Page = page, Limit = limit };
            CadResult result = await service.GetAllAsync(new(), new(), pagination).ConfigureAwait(false);
            int[] actualCadIds = result.Cads.Select(c => c.Id).ToArray();

            Assert.That(actualCadIds, Is.EqualTo(expectedCadIds),
                string.Format(IncorrectPaging, page));
        }

        [TestCase(Sorting.Newest)]
        [TestCase(Sorting.Oldest)]
        [TestCase(Sorting.Alphabetical)]
        [TestCase(Sorting.Unalphabetical)]
        [TestCase(Sorting.Category)]
        public async Task Test_ReturnsCorrectlyWithSorting(Sorting sorting)
        {
            var expectedSort = (sorting switch
            {
                Sorting.Newest => cads.OrderByDescending(c => c.CreationDate),
                Sorting.Oldest => cads.OrderBy(c => c.CreationDate),
                Sorting.Alphabetical => cads.OrderBy(c => c.Name),
                Sorting.Unalphabetical => cads.OrderByDescending(c => c.Name),
                Sorting.Category => cads.OrderBy(m => m.Category.Name),
                _ => cads.OrderBy(c => c.Id),
            }).Select(c => c.Id);

            CadResult result = await service.GetAllAsync(new(), new() { Sorting = sorting.ToString() }, new()).ConfigureAwait(false);
            var actualSort = result.Cads.Select(c => c.Id);

            Assert.That(actualSort, Is.EqualTo(expectedSort),
                message: string.Format(IncorrectSortByFilter, sorting.ToString()));
        }
    }
}
