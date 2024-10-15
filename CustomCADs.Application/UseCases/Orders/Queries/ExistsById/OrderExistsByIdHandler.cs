using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.ExistsById;

public class OrderExistsByIdHandler(IOrderQueries queries) : IRequestHandler<OrderExistsByIdQuery, bool>
{
    public async Task<bool> Handle(OrderExistsByIdQuery request, CancellationToken cancellationToken)
    {
        bool orderExists = await queries.ExistsByIdAsync(request.Id).ConfigureAwait(false);

        return orderExists;
    }
}
