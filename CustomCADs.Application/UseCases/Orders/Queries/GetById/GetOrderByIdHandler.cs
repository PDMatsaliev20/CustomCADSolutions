using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetById;

public class GetOrderByIdHandler(IOrderQueries queries) : IRequestHandler<GetOrderByIdQuery, OrderModel>
{
    public async Task<OrderModel> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(request.Id);

        var response = order.Adapt<OrderModel>();
        return response;
    }
}
