using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.IsBuyer;

public class IsOrderBuyerHandler(IOrderQueries queries) : IRequestHandler<IsOrderBuyerQuery, bool>
{
    public async Task<bool> Handle(IsOrderBuyerQuery req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        var response = order.Buyer.UserName == req.Username;
        return response;
    }
}
