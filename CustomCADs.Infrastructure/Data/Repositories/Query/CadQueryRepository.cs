using AutoMapper;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using CustomCADs.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CadQueryRepository(CadContext context, IMapper mapper) : IQueryRepository<Cad, int>
    {
        public async Task<IEnumerable<Cad>> GetAll(string? user = null, string? status = null, string? category = null, string? name = null, string? owner = null, string sorting = "", Func<Cad, bool>? customFilter = null, bool asNoTracking = false)
        {
            IQueryable<PCad> query = context.Cads
                .Query(asNoTracking)
                .Include(c => c.Category)
                .Include(c => c.Creator)
                .AsSplitQuery();

            if (user != null)
            {
                query = query.Where(c => c.Creator.UserName == user);
            }
            if (status != null && Enum.TryParse(status, ignoreCase: true, out CadStatus cadStatus))
            {
                query = query.Where(c => c.Status == cadStatus);
            }

            if (customFilter != null)
            {
                query = query.Where(c => customFilter(mapper.Map<Cad>(c)));
            }

            if (category != null)
            {
                query = query.Where(c => c.Category.Name == category);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                query = query.Where(c => c.Name.Contains(name));
            }
            if (!string.IsNullOrWhiteSpace(owner))
            {
                query = query.Where(c => c.Creator.UserName!.Contains(owner));
            }
            query = sorting.ToLower() switch
            {
                "newest" => query.OrderByDescending(c => c.CreationDate),
                "oldest" => query.OrderBy(c => c.CreationDate),
                "alphabetical" => query.OrderBy(c => c.Name),
                "unalphabetical" => query.OrderByDescending(c => c.Name),
                "category" => query.OrderBy(m => m.Category.Name),
                _ => query.OrderByDescending(c => c.Id),
            };

            PCad[] cads = await query.ToArrayAsync().ConfigureAwait(false);
            return cads.Select(mapper.Map<Cad>);
        }

        public async Task<Cad?> GetByIdAsync(int id, bool asNoTracking = false)
        {
            PCad? cad = await context.Cads
                .Query(asNoTracking)
                .Include(o => o.Category)
                .Include(o => o.Creator)
                .Include(o => o.Orders)
                .AsSplitQuery()
                .FirstOrDefaultAsync(c => c.Id == id)
                .ConfigureAwait(false);

            return mapper.Map<Cad?>(cad);
        }

        public async Task<bool> ExistsByIdAsync(int id)
            => await context.Cads.AnyAsync(o => o.Id == id).ConfigureAwait(false);

        public async Task<int> CountAsync(Func<Cad, bool> predicate, bool asNoTracking = false)
        {
            PCad[] cads = await context.Cads
                .Query(asNoTracking)
                .Include(c => c.Creator)
                .ToArrayAsync()
                .ConfigureAwait(false);

            return cads.Count((c) => predicate(mapper.Map<Cad>(c)));
        }
    }
}
