using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Orders;
using CustomCADs.Domain.Orders.Enums;
using CustomCADs.Domain.Orders.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetStatus;

public class SetOrderStatusHandler(IOrderQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetOrderStatusCommand>
{
    public async Task Handle(SetOrderStatusCommand req, CancellationToken ct)
    {
        Order order = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(req.Id);

        switch (req.Action)
        {
            case "begin":
                ArgumentNullException.ThrowIfNull(req.DesignerId, nameof(req.DesignerId));
                BeginOrder(order, req.DesignerId);
                break;

            case "report":
                ReportOrder(order);
                break;

            case "cancel":
                DesignerOrderRelationCheck(order.DesignerId, req.DesignerId);
                CancelOrder(order);
                break;

            case "finish":
                ArgumentNullException.ThrowIfNull(req.CadId, nameof(req.CadId));
                DesignerOrderRelationCheck(order.DesignerId, req.DesignerId);
                FinishOrder(order, req.CadId.Value);
                break;

            default: throw new OrderStatusException(req.Id, req.Action);
        }

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }

    private static void DesignerOrderRelationCheck(string? orderDesignerId, string? designerId)
    {
        if (orderDesignerId != designerId)
        {
            throw new DesignerNotAssociatedWithOrderException();
        }
    }

    private static void BeginOrder(Order order, string designerId)
    {
        if (order.Status != OrderStatus.Pending)
        {
            throw new OrderStatusException(order.Id, "Begin");
        }

        order.Status = OrderStatus.Begun;
        order.DesignerId = designerId;
    }

    private static void ReportOrder(Order order)
    {
        if (order.Status != OrderStatus.Pending)
        {
            throw new OrderStatusException(order.Id, "Report");
        }

        order.Status = OrderStatus.Reported;
    }

    private static void CancelOrder(Order order)
    {
        if (order.Status != OrderStatus.Begun)
        {
            throw new OrderStatusException(order.Id, "Cancel");
        }

        order.Status = OrderStatus.Pending;
    }

    private static void FinishOrder(Order order, int cadId)
    {
        if (order.Status != OrderStatus.Begun)
        {
            throw new OrderStatusException(order.Id, "Finish");
        }

        order.Status = OrderStatus.Pending;
        order.CadId = cadId;
    }
}
