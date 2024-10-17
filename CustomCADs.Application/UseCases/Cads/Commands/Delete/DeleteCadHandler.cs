using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Common.Helpers;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Enums;
using CustomCADs.Domain.Orders.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Delete;

public class DeleteCadHandler(
    ICadReads cadReads,
    IOrderReads orderReads,
    IWrites<Cad> cadWrites,
    IUnitOfWork uow) : IRequestHandler<DeleteCadCommand>
{
    public async Task Handle(DeleteCadCommand req, CancellationToken ct)
    {
        Cad cad = await cadReads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        ResetOrders(req.Id);
        cadWrites.Delete(cad);

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }

    private void ResetOrders(int id)
    {
        IEnumerable<Order> ordersWithCad = [ .. orderReads.GetAll().Filter(cadId: id)];

        foreach (Order order in ordersWithCad)
        {
            order.Status = OrderStatus.Pending;
            order.DesignerId = null;
            order.Designer = null;
            order.CadId = null;
            order.Cad = null;
        }
    }
}
