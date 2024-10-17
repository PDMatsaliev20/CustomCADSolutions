using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.IsBuyer;

public class IsOrderBuyerHandler(IOrderQueries queries) : IRequestHandler<IsOrderBuyerQuery, bool>
{
    public async Task<bool> Handle(IsOrderBuyerQuery request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(request.Id);

        var response = order.Buyer.UserName == request.Username;
        return response;
    }
}
