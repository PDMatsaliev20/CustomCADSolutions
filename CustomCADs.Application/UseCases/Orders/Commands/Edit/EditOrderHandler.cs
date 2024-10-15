using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Edit;

public class EditOrderHandler(IOrderQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditOrderCommand>
{
    public async Task Handle(EditOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(request.Id);

        order.Name = request.Model.Name;
        order.Description = request.Model.Description;
        order.ShouldBeDelivered = request.Model.ShouldBeDelivered;
        order.CategoryId = request.Model.CategoryId;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
