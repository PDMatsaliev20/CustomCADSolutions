using CustomCADs.Domain.Entities;

namespace CustomCADs.Application.Common.Helpers;

public static class Searcher
{
    public static IQueryable<Cad> Search(this IQueryable<Cad> query, string? category = null, string? name = null, string? creator = null)
    {
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(c => c.Category.Name == category);
        }
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(c => c.Name.Contains(name));
        }
        if (!string.IsNullOrWhiteSpace(creator))
        {
            query = query.Where(c => c.Creator.UserName.Contains(creator));
        }

        return query;
    }

    public static IQueryable<Order> Search(this IQueryable<Order> query, string? category = null, string? name = null)
    {
        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(o => o.Category.Name == category);
        }
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(o => o.Name.Contains(name));
        }

        return query;
    }

    public static IQueryable<User> Search(this IQueryable<User> query, string? username = null, string? email = null, string? firstName = null, string? lastName = null, DateTime? rtEndDateBefore = null, DateTime? rtEndDateAfter = null)
    {
        if (!string.IsNullOrWhiteSpace(username))
        {
            query = query.Where(u => u.UserName.Contains(username));
        }
        if (!string.IsNullOrWhiteSpace(email))
        {
            query = query.Where(u => u.Email.Contains(email));
        }
        if (!string.IsNullOrWhiteSpace(firstName))
        {
            query = query.Where(u => u.FirstName != null && u.FirstName.Contains(firstName));
        }
        if (!string.IsNullOrWhiteSpace(lastName))
        {
            query = query.Where(u => u.LastName != null && u.LastName.Contains(lastName));
        }
        if (rtEndDateBefore.HasValue)
        {
            query = query.Where(u => u.RefreshTokenEndDate.HasValue && u.RefreshTokenEndDate < rtEndDateBefore);
        }
        if (rtEndDateAfter.HasValue)
        {
            query = query.Where(u => u.RefreshTokenEndDate.HasValue && u.RefreshTokenEndDate < rtEndDateAfter);
        }

        return query;
    }

    public static IQueryable<Role> Search(this IQueryable<Role> query, string? name = null, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
        {
            query = query.Where(r => r.Name.Contains(name));
        }
        if (!string.IsNullOrWhiteSpace(description))
        {
            query = query.Where(r => r.Description.Contains(description));
        }

        return query;
    }
}
