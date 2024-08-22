using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CustomCADs.Application.Services
{
    public class DesignerService(IRepository repository, IMapper mapper) : IDesignerService
    {
        public async Task<OrderResult> GetOrdersAsync(string status, string? designerId, SearchModel search, PaginationModel pagination)
        {
            IQueryable<Order> dbOrders = repository.All<Order>();

            if (string.IsNullOrWhiteSpace(designerId))
            {
                dbOrders = dbOrders.Where(o => o.DesignerId == designerId);
            }

            dbOrders = status.ToLower() switch
            {
                "pending" => dbOrders.Where(o => o.Status == OrderStatus.Pending),
                "begun" => dbOrders.Where(o => o.Status == OrderStatus.Begun),
                "finished" => dbOrders.Where(o => o.Status == OrderStatus.Finished && o.ShouldBeDelivered),
                _ => dbOrders
            };

            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                dbOrders = dbOrders.Where(o => o.Name.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Owner))
            {
                dbOrders = dbOrders.Where(o => o.Buyer.UserName!.Contains(search.Owner));
            }
            if (!string.IsNullOrWhiteSpace(search.Category))
            {
                dbOrders = dbOrders.Where(o => o.Category.Name == search.Category);
            }
            dbOrders = search.Sorting.ToLower() switch
            {
                "newest" => dbOrders.OrderByDescending(o => o.OrderDate),
                "oldest" => dbOrders.OrderBy(o => o.OrderDate),
                "alphabetical" => dbOrders.OrderBy(o => o.Name),
                "unalphabetical" => dbOrders.OrderByDescending(o => o.Name),
                "category" => dbOrders.OrderBy(o => o.Category.Name),
                _ => dbOrders.OrderByDescending(o => o.Id)
            };

            // Pagination
            Order[] orders = await dbOrders
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
                .ToArrayAsync()
                .ConfigureAwait(false);

            OrderModel[] models = mapper.Map<OrderModel[]>(orders);
            return new()
            {
                Count = dbOrders.Count(),
                Orders = models
            };
        }

        public async Task EditCadStatusAsync(int id, CadStatus status)
        {
            Cad cad = await repository.GetByIdAsync<Cad>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException("Model doesn't exist!");

            cad.Status = status;
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<CadResult> GetCadsAsync(SearchModel search, PaginationModel pagination)
        {
            IQueryable<Cad> allCads = repository.All<Cad>().Where(c => c.Status == CadStatus.Unchecked);

            // Search & Sort
            if (search.Category != null)
            {
                allCads = allCads.Where(c => c.Category.Name == search.Category);
            }
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                allCads = allCads.Where(c => c.Name.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.Owner))
            {
                allCads = allCads.Where(c => c.Creator.UserName!.Contains(search.Owner));
            }

            allCads = search.Sorting.ToLower() switch
            {
                "newest" => allCads.OrderByDescending(c => c.CreationDate),
                "oldest" => allCads.OrderBy(c => c.CreationDate),
                "alphabetical" => allCads.OrderBy(c => c.Name),
                "unalphabetical" => allCads.OrderByDescending(c => c.Name),
                "category" => allCads.OrderBy(m => m.Category.Name),
                _ => allCads.OrderByDescending(c => c.Id),
            };

            Cad[] cads = await allCads
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
                .ToArrayAsync()
                .ConfigureAwait(false);

            CadModel[] models = mapper.Map<CadModel[]>(cads);
            return new()
            {
                Count = allCads.Count(),
                Cads = models,
            };
        }

        public async Task BeginAsync(int id, string designerId)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Begun;
            order.DesignerId = designerId;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ReportAsync(int id)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
            ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Reported;
            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CancelAsync(int id, string designerId)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            if (order.DesignerId != designerId)
            {
                throw new UnauthorizedAccessException();
            }

            order.DesignerId = null;
            order.Status = OrderStatus.Pending;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CompleteAsync(int id, int cadId, string designerId)
        {
            Order order = await repository.GetByIdAsync<Order>(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            if (order.DesignerId != designerId)
            {
                throw new UnauthorizedAccessException();
            }

            order.CadId = cadId;
            order.Status = OrderStatus.Finished;

            await repository.SaveChangesAsync().ConfigureAwait(false);
        }

    }
}
