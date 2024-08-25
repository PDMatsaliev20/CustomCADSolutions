using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CategoryQueryRepository(CadContext context) : IQueryRepository<Category>
    {
        public IQueryable<Category> GetAll()
        {
            return context.Categories;
        }

        public async Task<Category?> GetByIdAsync(object id)
        {
            return await context.Categories.FindAsync(id).ConfigureAwait(false);
        }

        public int Count(Func<Category, bool> predicate)
        {
            return context.Categories.Count(predicate);
        }
    }
}
