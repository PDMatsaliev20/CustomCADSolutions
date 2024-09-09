using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.Models.Utilities;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Services
{
    public class DesignerService(IDbTracker dbTracker,
        IQueries<Cad> cadQueries, 
        IQueries<Order> orderQueries,
        IMapper mapper) : IDesignerService
    {
        public async Task<OrderResult> GetOrdersAsync(string status, string? designerId, SearchModel search, PaginationModel pagination)
        {
            IEnumerable<Order> orders = await orderQueries.GetAll(
                status: status,
                name: search.Name,
                owner: search.Owner,
                category: search.Category,
                sorting: search.Sorting,
                customFilter: o => o.DesignerId == designerId,
                asNoTracking: true
            ).ConfigureAwait(false);

            OrderModel[] models = mapper.Map<OrderModel[]>(orders
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
            );

            return new()
            {
                Count = orders.Count(),
                Orders = models
            };
        }

        public async Task EditCadStatusAsync(int id, CadStatus status)
        {
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException("Model doesn't exist!");

            cad.Status = status;
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<CadResult> GetCadsAsync(SearchModel search, PaginationModel pagination)
        {
            IEnumerable<Cad> cads = await cadQueries.GetAll(
                status: CadStatus.Unchecked.ToString(),
                category: search.Category,
                name: search.Name,
                owner: search.Owner,
                sorting: search.Sorting,
                asNoTracking: true
            ).ConfigureAwait(false);

            CadModel[] models = mapper.Map<CadModel[]>(cads
                .Skip((pagination.Page - 1) * pagination.Limit)
                .Take(pagination.Limit)
            );

            return new()
            {
                Count = cads.Count(),
                Cads = models,
            };
        }

        public async Task BeginAsync(int id, string designerId)
        {
            Order order = await orderQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Begun;
            order.DesignerId = designerId;

            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task ReportAsync(int id)
        {
            Order order = await orderQueries.GetByIdAsync(id).ConfigureAwait(false)
            ?? throw new KeyNotFoundException();

            order.Status = OrderStatus.Reported;
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CancelAsync(int id, string designerId)
        {
            Order order = await orderQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            if (order.DesignerId != designerId)
            {
                throw new UnauthorizedAccessException();
            }

            order.DesignerId = null;
            order.Status = OrderStatus.Pending;

            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task CompleteAsync(int id, int cadId, string designerId)
        {
            Order order = await orderQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException();

            if (order.DesignerId != designerId)
            {
                throw new UnauthorizedAccessException();
            }

            order.CadId = cadId;
            order.Status = OrderStatus.Finished;

            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
