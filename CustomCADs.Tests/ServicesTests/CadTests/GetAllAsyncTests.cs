using CustomCADs.Core.Models;
using CustomCADs.Domain.Entities.Enums;
using static CustomCADs.Core.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class GetAllAsyncTests : BaseCadsTests
    {
        [Test]
        public async Task Test_ReturnsAllWithNoFilters()
        {
            int expectedCount = this.cads.Length;
            CadQueryModel query = new();

            CadQueryResult result = await service.GetAllAsync(query);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "None"));
        }

        [TestCase("Category1")]
        [TestCase("Category4")]
        public async Task Test_ReturnsCorrectlyWithCategoryFilter(string categoryName)
        {
            int expectedCount = this.cads.Count(c => c.Category.Name == categoryName);
            CadQueryModel query = new() { Category = categoryName };

            CadQueryResult result = await service.GetAllAsync(query);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Category"));
        }

        [TestCase("Client")]
        [TestCase("Contributor")]
        public async Task Test_ReturnsCorrectlyWithCreatorFilter(string creatorName)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName == creatorName);
            CadQueryModel query = new() { Creator = creatorName };

            CadQueryResult result = await service.GetAllAsync(query);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Creator"));
        }

        [TestCase("Cad")]
        [TestCase("3")]
        public async Task Test_ReturnsCorrectlyWithLikeNameFilter(string likeName)
        {
            int expectedCount = this.cads.Count(c => c.Name.Contains(likeName));
            CadQueryModel query = new() { SearchName = likeName };

            CadQueryResult result = await service.GetAllAsync(query);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeName"));
        }

        [TestCase("C")]
        [TestCase("ient")]
        [TestCase("ontr")]
        public async Task Test_ReturnsCorrectlyWithLikeCreatorFilter(string likeCreator)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName.Contains(likeCreator));
            CadQueryModel query = new() { SearchCreator = likeCreator };

            CadQueryResult result = await service.GetAllAsync(query);
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

            CadQueryModel query = new() { Status = status };

            CadQueryResult result = await service.GetAllAsync(query);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Status"));
        }

        [TestCase(1, 3)]
        [TestCase(2, 3)]
        [TestCase(3, 3)]
        [TestCase(1, 2)]
        [TestCase(4, 2)]
        public async Task Test_ReturnsCorrectlyWithPagination(int currentPage, int cadsPerPage)
        {
            int[] expectedCadIds = this.cads
                .OrderBy(c => c.Id)
                .Skip((currentPage - 1) * cadsPerPage)
                .Take(cadsPerPage)
                .Select(c => c.Id)
                .ToArray();

            CadQueryModel query = new() { CurrentPage = currentPage, CadsPerPage = cadsPerPage };
            int[] actualCadIds = (await service.GetAllAsync(query)).Cads
                .Select(c => c.Id)
                .ToArray();

            Assert.That(actualCadIds, Is.EqualTo(expectedCadIds),
                string.Format(IncorrectPaging, currentPage));
        }

        [TestCase(CadSorting.Newest)]
        [TestCase(CadSorting.Alphabetical)]
        [TestCase(CadSorting.Category)]
        public async Task Test_ReturnsCorrectlyWithSorting(CadSorting sorting)
        {
            var expectedSort = (sorting switch
            {
                CadSorting.Newest => cads.OrderBy(c => c.CreationDate),
                CadSorting.Oldest => cads.OrderByDescending(c => c.CreationDate),
                CadSorting.Alphabetical => cads.OrderBy(c => c.Name),
                CadSorting.Unalphabetical => cads.OrderByDescending(c => c.Name),
                CadSorting.Category => cads.OrderBy(m => m.Category.Name),
                _ => cads.OrderBy(c => c.Id),
            }).Select(c => c.Id);
            CadQueryModel query = new() { Sorting = sorting, CadsPerPage = 8 };

            CadQueryResult result = await service.GetAllAsync(query);
            var actualSort = result.Cads.Select(c => c.Id);

            Assert.That(actualSort, Is.EqualTo(expectedSort),
                message: string.Format(IncorrectSortByFilter, sorting.ToString()));
        }
    }
}
