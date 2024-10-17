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
    public async Task<int> Handle(CreateOrderCommand req, CancellationToken ct)
    {
        bool categoryExists = await queries.ExistsByIdAsync(req.Model.CategoryId, ct).ConfigureAwait(false);
        if (!categoryExists)
        {
            throw new CategoryNotFoundException(req.Model.CategoryId);
        }

        Order order = req.Model.Adapt<Order>();
        await commands.AddAsync(order, ct).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return order.Id;
    }
}
