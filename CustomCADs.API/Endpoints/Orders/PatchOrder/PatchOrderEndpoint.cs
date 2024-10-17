using CustomCADs.API.Helpers;
using CustomCADs.Application.UseCases.Orders.Commands.SetShouldBeDelivered;
using CustomCADs.Application.UseCases.Orders.Queries.IsBuyer;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.PatchOrder;

using static ApiMessages;
using static StatusCodes;

public class PatchOrderEndpoint(IMediator mediator) : Endpoint<PatchOrderRequest>
{
    public override void Configure()
    {
        Patch("{id}");
        Group<OrdersGroup>();
        Description(d => d
            .WithSummary("Updates Order with an array of operations.")
            .Produces<EmptyResponse>(Status204NoContent));
    }

    public override async Task HandleAsync(PatchOrderRequest req, CancellationToken ct)
    {
        IsOrderBuyerQuery isBuyerQuery = new(req.Id, User.GetName());
        bool userIsBuyer = await mediator.Send(isBuyerQuery, ct).ConfigureAwait(false);

        if (!userIsBuyer)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = ForbiddenAccess,
            });
            await SendErrorsAsync().ConfigureAwait(false);
        }

        SetOrderShouldBeDeliveredCommand command = new(req.Id, req.ShouldBeDelivered);
        await mediator.Send(command, ct).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
