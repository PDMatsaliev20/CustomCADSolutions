using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using MediatR;

namespace CustomCADs.Application.UseCases.Orders.Commands.SetStatus;

public class SetOrderStatusHandler(IOrderQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetOrderStatusCommand>
{
    public async Task Handle(SetOrderStatusCommand request, CancellationToken cancellationToken)
    {
        Order order = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new OrderNotFoundException(request.Id);

        switch (request.Action)
        {
            case "begin":
                ArgumentNullException.ThrowIfNull(request.DesignerId, nameof(request.DesignerId));
                BeginOrder(order, request.DesignerId);
                break;

            case "report":
                ReportOrder(order);
                break;

            case "cancel":
                DesignerOrderRelationCheck(order.DesignerId, request.DesignerId);
                CancelOrder(order);
                break;

            case "finish":
                ArgumentNullException.ThrowIfNull(request.CadId, nameof(request.CadId));
                DesignerOrderRelationCheck(order.DesignerId, request.DesignerId);
                FinishOrder(order, request.CadId.Value);
                break;

            default: throw new OrderStatusException(request.Id, request.Action);
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
