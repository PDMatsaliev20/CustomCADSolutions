using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.IsBuyer;

public class IsOrderBuyerHandler(IOrderReads reads) : IRequestHandler<IsOrderBuyerQuery, bool>
{
    public async Task<bool> Handle(IsOrderBuyerQuery req, CancellationToken ct)
    {
        Order order = await reads.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        var response = order.Buyer.UserName == req.Username;
        return response;
    }
}
