using AutoMapper;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Infrastructure.Data.Entities;

namespace CustomCADs.Infrastructure.Data.Repositories.Command
{
    public class CategoryCommandRepository(CadContext context, IMapper mapper) : ICommandRepository<Category>
    {
        public async Task<Category> AddAsync(Category category)
        {
            PCategory entity = mapper.Map<PCategory>(category);
            var entry = await context.Categories.AddAsync(entity).ConfigureAwait(false);
            return mapper.Map<Category>(entry.Entity);
        }

        public async Task AddRangeAsync(params Category[] entity)
            => await context.Categories.AddRangeAsync(mapper.Map<PCategory[]>(entity)).ConfigureAwait(false);

        public void Delete(Category entity)
            => context.Categories.Remove(mapper.Map<PCategory>(entity));

        public void DeleteRange(params Category[] entity)
            => context.Categories.RemoveRange(mapper.Map<PCategory[]>(entity));
    }
}
