using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetAll;

public class GetAllOrdersHandler(IOrderQueries queries) : IRequestHandler<GetAllOrdersQuery, OrderResult>
{
    public Task<OrderResult> Handle(GetAllOrdersQuery req, CancellationToken ct)
    {
        IQueryable<Order> queryable = queries.GetAll(asNoTracking: true)
            .Filter(req.Buyer, req.Status)
            .Search(req.Category, req.Name)
            .Sort(req.Sorting);

        IEnumerable<Order> orders =
        [
            .. queryable
            .Skip((req.Page - 1) * req.Limit)
            .Take(req.Limit)
        ];

        OrderResult response = new()
        {
            Count = queryable.Count(),
            Orders = orders.Adapt<ICollection<OrderModel>>(),
        };
        return Task.FromResult(response);
    }
}
