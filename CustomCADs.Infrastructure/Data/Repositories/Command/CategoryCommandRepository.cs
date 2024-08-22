using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;

namespace CustomCADs.Infrastructure.Data.Repositories.Command
{
    public class CategoryCommandRepository(CadContext context) : ICommandRepository<Category>
    {
        public async Task<Category> AddAsync(Category entity) 
            => (await context.AddAsync(entity).ConfigureAwait(false)).Entity;
        
        public async Task AddRangeAsync(params Category[] entity) 
            => await context.AddRangeAsync(entity).ConfigureAwait(false);

        public void Delete(Category entity) 
            => context.Remove(entity);
        
        public void DeleteRange(params Category[] entity) 
            => context.RemoveRange(entity);
    }
}
