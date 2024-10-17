using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetById;

public class GetOrderByIdHandler(IOrderQueries queries) : IRequestHandler<GetOrderByIdQuery, OrderModel>
{
    public async Task<OrderModel> Handle(GetOrderByIdQuery req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        var response = order.Adapt<OrderModel>();
        return response;
    }
}
