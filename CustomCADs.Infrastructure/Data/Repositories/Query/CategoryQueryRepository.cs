﻿using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.Infrastructure.Data.Repositories.Query
{
    public class CategoryQueryRepository(CadContext context) : IQueryRepository<Category>
    {
        public IQueryable<Category> GetAll(bool asNoTracking = false)
        {
            return Query(context.Categories, asNoTracking);
        }

        public async Task<Category?> GetByIdAsync(object id, bool asNoTracking = false)
        {
            return await Query(context.Categories, asNoTracking)
                .FirstOrDefaultAsync(c => id.Equals(c.Id))
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsByIdAsync(object id)
            => await context.Categories.AnyAsync(o => id.Equals(o.Id)).ConfigureAwait(false);

        public int Count(Func<Category, bool> predicate, bool asNoTracking = false)
        {
            return Query(context.Categories, asNoTracking)
                .Count(predicate);
        }

        private static IQueryable<Category> Query(DbSet<Category> categories, bool asNoTracking)
            => asNoTracking ? categories.AsNoTracking() : categories;
    }
}
