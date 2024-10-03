using AutoMapper;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;

namespace CustomCADs.Application.Services
{
    public class DesignerService(IDbTracker dbTracker,
        ICadQueries cadQueries,
        IOrderQueries orderQueries,
        IMapper mapper) : IDesignerService
    {
        public OrderResult GetOrders(string status = "", int? id = null, string? designerId = null, string? category = null, string? name = null, string? buyer = null, string sorting = "", int page = 1, int limit = 20)
        {
            IQueryable<Order> queryable = orderQueries.GetAll(true);
            if (id != null)
            {
                queryable = queryable.Where(x => x.Id == id);
                return new()
                {
                    Count = queryable.Count(),
                    Orders = mapper.Map<OrderModel[]>(queryable.ToArray())
                };
            }

            queryable = queryable.Filter(status: status, customFilter: string.IsNullOrEmpty(designerId) ? null : o => o.DesignerId == designerId);
            queryable = queryable.Search(category: category, name: name, buyer: buyer);
            queryable = queryable.Sort(sorting: sorting);


            IEnumerable<Order> orders = queryable.Skip((page - 1) * limit).Take(limit);
            return new()
            {
                Count = orders.Count(),
                Orders = mapper.Map<OrderModel[]>(orders),
            };
        }

        public async Task<(int? PrevId, CadModel Current, int? NextId)> GetNextCurrentAndPreviousByIdAsync(int id)
        {
            List<Cad> cads = [.. cadQueries.GetAll(true).OrderBy(c => c.Id)];

            Cad cad = cads.FirstOrDefault(c => c.Id == id)
                ?? throw new KeyNotFoundException();
            int cadIndex = cads.IndexOf(cad);

            int? prevId = null;
            if (cad.Id != (cads.FirstOrDefault()?.Id ?? 0))
            {
                prevId = cads[cadIndex - 1].Id;   
            }
            
            int? nextId = null;
            if (cad.Id != (cads.LastOrDefault()?.Id ?? 0))
            {
                nextId = cads[cadIndex + 1].Id;                   
            }

            return (prevId, mapper.Map<CadModel>(cad), nextId);
        }

        public async Task EditCadStatusAsync(int id, CadStatus status)
        {
            Cad cad = await cadQueries.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException("Model doesn't exist!");

            cad.Status = status;
            await dbTracker.SaveChangesAsync().ConfigureAwait(false);
        }

        public CadResult GetCadsAsync(string? category = null, string? name = null, string? creator = null, string sorting = "", int page = 1, int limit = 20)
        {
            IQueryable<Cad> queryable = cadQueries.GetAll(true);
            queryable = queryable.Filter(status: CadStatus.Unchecked.ToString());
            queryable = queryable.Search(category: category, name: name, creator: creator);
            queryable = queryable.Sort(sorting);

            IEnumerable<Cad> cads = [.. queryable.Skip((page - 1) * limit).Take(limit)];
            return new()
            {
                Count = cads.Count(),
                Cads = mapper.Map<CadModel[]>(cads),
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
