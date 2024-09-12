using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Enums;
using static CustomCADs.Application.TestsErrorMessages;

namespace CustomCADs.Tests.ServicesTests.CadTests
{
    public class GetAllAsyncTests : BaseCadsTests
    {
        [Test]
        public void Test_ReturnsAllWithNoFilters()
        {
            int expectedCount = this.cads.Length;
            
            CadResult result = service.GetAllAsync();
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "None"));
        }

        [TestCase("Category1")]
        [TestCase("Category4")]
        public void Test_ReturnsCorrectlyWithCategoryFilter(string categoryName)
        {
            int expectedCount = this.cads.Count(c => c.Category.Name == categoryName);
            CadResult result = service.GetAllAsync(category: categoryName);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Category"));
        }

        [TestCase("Client")]
        [TestCase("Contributor")]
        public void Test_ReturnsCorrectlyWithCreatorFilter(string creatorName)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName == creatorName);
            CadResult result = service.GetAllAsync(creator: creatorName);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Creator"));
        }

        [TestCase("Cad")]
        [TestCase("3")]
        public void Test_ReturnsCorrectlyWithLikeNameFilter(string likeName)
        {
            int expectedCount = this.cads.Count(c => c.Name.Contains(likeName));
            CadResult result = service.GetAllAsync(name: likeName);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeName"));
        }

        [TestCase("C")]
        [TestCase("ient")]
        [TestCase("ontr")]
        public void Test_ReturnsCorrectlyWithLikeCreatorFilter(string likeCreator)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName!.Contains(likeCreator));
            SearchModel search = new() { Owner = likeCreator };

            CadResult result = service.GetAllAsync(owner: likeCreator);
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeCreator"));
        }

        [TestCase(CadStatus.Unchecked)]
        [TestCase(CadStatus.Validated)]
        [TestCase(CadStatus.Reported)]
        [TestCase(CadStatus.Banned)]
        [TestCase(null)]
        public void Test_ReturnsCorrectlyWithStatusFilters(CadStatus? status)
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

            CadResult result = service.GetAllAsync(status: status == null ? null : status.ToString());
            int actualCount = result.Count;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Status"));
        }

        [TestCase(1, 3)]
        [TestCase(2, 3)]
        [TestCase(3, 3)]
        [TestCase(1, 2)]
        [TestCase(4, 2)]
        public void Test_ReturnsCorrectlyWithPagination(int page, int limit)
        {
            int[] expectedCadIds = this.cads
                .OrderByDescending(c => c.Id)
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(c => c.Id)
                .ToArray();

            CadResult result = service.GetAllAsync(page: page, limit: limit);
            int[] actualCadIds = result.Cads.Select(c => c.Id).ToArray();

            Assert.That(actualCadIds, Is.EqualTo(expectedCadIds),
                string.Format(IncorrectPaging, page));
        }

        [TestCase(Sorting.Newest)]
        [TestCase(Sorting.Oldest)]
        [TestCase(Sorting.Alphabetical)]
        [TestCase(Sorting.Unalphabetical)]
        [TestCase(Sorting.Category)]
        public void Test_ReturnsCorrectlyWithSorting(Sorting sorting)
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

            CadResult result = service.GetAllAsync(sorting: sorting.ToString());
            var actualSort = result.Cads.Select(c => c.Id);

            Assert.That(actualSort, Is.EqualTo(expectedSort),
                message: string.Format(IncorrectSortByFilter, sorting.ToString()));
        }
    }
}
