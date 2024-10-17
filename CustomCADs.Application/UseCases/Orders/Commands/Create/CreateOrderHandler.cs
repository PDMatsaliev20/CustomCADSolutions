using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Create;

public class CreateOrderHandler(
    ICategoryQueries queries, 
    ICommands<Order> commands, 
    IUnitOfWork unitOfWork) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        bool categoryExists = await queries.ExistsByIdAsync(request.Model.CategoryId).ConfigureAwait(false);
        if (!categoryExists)
        {
            throw new CategoryNotFoundException(request.Model.CategoryId);
        }

        Order order = request.Model.Adapt<Order>();
        await commands.AddAsync(order).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return order.Id;
    }
}
