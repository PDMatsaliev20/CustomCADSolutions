using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.HasCadById;

public class OrderHasCadByIdHandler(IOrderQueries queries) : IRequestHandler<OrderHasCadByIdQuery, bool>
{
    public async Task<bool> Handle(OrderHasCadByIdQuery req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        var response = order.CadId != null && order.Cad != null;
        return response;
    }
}
