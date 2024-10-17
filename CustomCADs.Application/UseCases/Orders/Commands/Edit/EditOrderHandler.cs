using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Edit;

public class EditOrderHandler(IOrderQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditOrderCommand>
{
    public async Task Handle(EditOrderCommand req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        order.Name = req.Model.Name;
        order.Description = req.Model.Description;
        order.ShouldBeDelivered = req.Model.ShouldBeDelivered;
        order.CategoryId = req.Model.CategoryId;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
