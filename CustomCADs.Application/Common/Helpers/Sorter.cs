using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Users;

namespace CustomCADs.Application.Common.Helpers;

public static class Sorter
{
    public static IQueryable<Cad> Sort(this IQueryable<Cad> query, string sorting = "")
    {
        query = sorting.ToLower() switch
        {
            "newest" => query.OrderByDescending(c => c.CreationDate),
            "oldest" => query.OrderBy(c => c.CreationDate),
            "alphabetical" => query.OrderBy(c => c.Name),
            "unalphabetical" => query.OrderByDescending(c => c.Name),
            "category" => query.OrderBy(m => m.Category.Name),
            _ => query.OrderByDescending(c => c.Id),
        };

        return query;
    }

    public static IQueryable<Order> Sort(this IQueryable<Order> query, string sorting = "")
    {
        query = sorting.ToLower() switch
        {
            "newest" => query.OrderByDescending(o => o.OrderDate),
            "oldest" => query.OrderBy(o => o.OrderDate),
            "alphabetical" => query.OrderBy(o => o.Name),
            "unalphabetical" => query.OrderByDescending(o => o.Name),
            "category" => query.OrderBy(o => o.Category.Name),
            _ => query.OrderByDescending(o => o.Id),
        };

        return query;
    }

    public static IQueryable<Category> Sort(this IQueryable<Category> query, string sorting = "")
    {
        query = sorting.ToLower() switch
        {
            "newest" => query.OrderByDescending(c => c.Id),
            "oldest" => query.OrderBy(c => c.Id),
            "alphabetical" => query.OrderBy(c => c.Name),
            "unalphabetical" => query.OrderByDescending(c => c.Name),
            _ => query.OrderByDescending(c => c.Id),
        };

        return query;
    }

    public static IQueryable<User> Sort(this IQueryable<User> query, string sorting = "")
    {
        query = sorting.ToLower() switch
        {
            "newest" => query.OrderByDescending(u => u.Id),
            "oldest" => query.OrderBy(u => u.Id),
            "alphabetical" => query.OrderBy(u => u.UserName),
            "unalphabetical" => query.OrderByDescending(u => u.UserName),
            _ => query.OrderByDescending(u => u.Id),
        };

        return query;
    }

    public static IQueryable<Role> Sort(this IQueryable<Role> query, string sorting = "")
    {
        query = sorting.ToLower() switch
        {
            "newest" => query.OrderByDescending(r => r.Id),
            "oldest" => query.OrderBy(r => r.Id),
            "alphabetical" => query.OrderBy(r => r.Name),
            "unalphabetical" => query.OrderByDescending(r => r.Name),
            _ => query.OrderByDescending(r => r.Id),
        };

        return query;
    }
}
