using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.ExistsById;

public class OrderExistsByIdHandler(IOrderQueries queries) : IRequestHandler<OrderExistsByIdQuery, bool>
{
    public async Task<bool> Handle(OrderExistsByIdQuery req, CancellationToken ct)
    {
        bool orderExists = await queries.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return orderExists;
    }
}
