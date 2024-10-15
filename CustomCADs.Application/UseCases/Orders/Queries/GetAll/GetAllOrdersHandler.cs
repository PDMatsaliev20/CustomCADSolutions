using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetAll
{
    public class GetAllOrdersHandler(IOrderQueries queries) : IRequestHandler<GetAllOrdersQuery, OrderResult>
    {
        public Task<OrderResult> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            IQueryable<Order> queryable = queries.GetAll(true)
                .Filter(request.Buyer, request.Status)
                .Search(request.Category, request.Name)
                .Sort(request.Sorting);

            IEnumerable<Order> orders =
            [
                .. queryable
                .Skip((request.Page - 1) * request.Limit)
                .Take(request.Limit)
            ];

            OrderResult response = new()
            {
                Count = queryable.Count(),
                Orders = orders.Adapt<ICollection<OrderModel>>(),
            };
            return Task.FromResult(response);
        }
    }
}
