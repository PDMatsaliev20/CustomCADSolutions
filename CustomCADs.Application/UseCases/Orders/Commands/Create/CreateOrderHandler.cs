using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Categories.Reads;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Shared;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.Create;

public class CreateOrderHandler(
    ICategoryReads categoryReads, 
    IWrites<Order> orderWrites, 
    IUnitOfWork uow) : IRequestHandler<CreateOrderCommand, int>
{
    public async Task<int> Handle(CreateOrderCommand req, CancellationToken ct)
    {
        bool categoryExists = await categoryReads.ExistsByIdAsync(req.Model.CategoryId, ct).ConfigureAwait(false);
        if (!categoryExists)
        {
            throw new CategoryNotFoundException(req.Model.CategoryId);
        }

        Order order = req.Model.Adapt<Order>();
        await orderWrites.AddAsync(order, ct).ConfigureAwait(false);
        await uow.SaveChangesAsync().ConfigureAwait(false);

        return order.Id;
    }
}
