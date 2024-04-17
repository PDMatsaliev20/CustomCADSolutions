using static CustomCADSolutions.Core.TestsErrorMessages;
using CustomCADSolutions.Core.Models;
using CustomCADSolutions.Infrastructure.Data.Models.Enums;

namespace CustomCADSolutions.Tests.ServicesTests.CadTests
{
    public class GetAllAsyncTests : BaseCadsTests
    {
        [Test]
        public async Task Test_ReturnsAllWithNoFilters()
        {
            int expectedCount = this.cads.Length;
            CadQueryModel query = new();

            query = await service.GetAllAsync(query);
            int actualCount = query.TotalCount;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "None"));
        }

        [TestCase("Category1")]
        [TestCase("Category4")]
        public async Task Test_ReturnsCorrectlyWithCategoryFilter(string categoryName)
        {
            int expectedCount = this.cads.Count(c => c.Category.Name == categoryName);
            CadQueryModel query = new() { Category = categoryName };

            query = await service.GetAllAsync(query);
            int actualCount = query.TotalCount;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Category"));
        }

        [TestCase("Client")]
        [TestCase("Contributor")]
        public async Task Test_ReturnsCorrectlyWithCreatorFilter(string creatorName)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName == creatorName);
            CadQueryModel query = new() { Creator = creatorName };

            query = await service.GetAllAsync(query);
            int actualCount = query.TotalCount;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Creator"));
        }

        [TestCase("Cad")]
        [TestCase("3")]
        public async Task Test_ReturnsCorrectlyWithLikeNameFilter(string likeName)
        {
            int expectedCount = this.cads.Count(c => c.Name.Contains(likeName));
            CadQueryModel query = new() { LikeName = likeName };

            query = await service.GetAllAsync(query);
            int actualCount = query.TotalCount;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeName"));
        }

        [TestCase("C")]
        [TestCase("ient")]
        [TestCase("ontr")]
        public async Task Test_ReturnsCorrectlyWithLikeCreatorFilter(string likeCreator)
        {
            int expectedCount = this.cads.Count(c => c.Creator.UserName.Contains(likeCreator));
            CadQueryModel query = new() { LikeCreator = likeCreator };

            query = await service.GetAllAsync(query);
            int actualCount = query.TotalCount;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "LikeCreator"));
        }

        [TestCase(false, false)]
        [TestCase(false, true)]
        [TestCase(true, false)]
        [TestCase(true, true)]
        public async Task Test_ReturnsCorrectlyWithValidationFilters(bool validated, bool unvalidated)
        {
            int expectedCount = 0;
            if (validated ^ unvalidated)
            {
                if (validated)
                {
                    expectedCount = cads.Count(c => c.IsValidated);
                }

                if (unvalidated)
                {
                    expectedCount = cads.Count(c => !c.IsValidated);
                }
            }
            else if (validated && unvalidated)
            {
                expectedCount = cads.Length;
            }

            CadQueryModel query = new() { Validated = validated, Unvalidated = unvalidated };

            query = await service.GetAllAsync(query);
            int actualCount = query.TotalCount;

            Assert.That(actualCount, Is.EqualTo(expectedCount),
                message: string.Format(IncorrectCountByFilter, "Validated and Unvalidated"));
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

            query = await service.GetAllAsync(query);
            var actualSort = query.Cads.Select(c => c.Id);

            Assert.That(actualSort, Is.EqualTo(expectedSort),
                message: string.Format(IncorrectSortByFilter, sorting.ToString()));
        }
    }
}
