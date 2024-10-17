using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.GetById;

public class GetOrderByIdHandler(IOrderReads reads) : IRequestHandler<GetOrderByIdQuery, OrderModel>
{
    public async Task<OrderModel> Handle(GetOrderByIdQuery req, CancellationToken ct)
    {
        Order order = await reads.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        var response = order.Adapt<OrderModel>();
        return response;
    }
}
