using CustomCADs.Domain.Orders.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Queries.ExistsById;

public class OrderExistsByIdHandler(IOrderReads reads) : IRequestHandler<OrderExistsByIdQuery, bool>
{
    public async Task<bool> Handle(OrderExistsByIdQuery req, CancellationToken ct)
    {
        bool orderExists = await reads.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return orderExists;
    }
}
