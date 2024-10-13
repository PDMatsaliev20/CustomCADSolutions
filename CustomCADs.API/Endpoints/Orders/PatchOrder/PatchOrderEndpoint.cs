using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.PatchOrder
{
    using static ApiMessages;
    using static StatusCodes;

    public class PatchOrderEndpoint(IOrderService service) : Endpoint<PatchOrderRequest>
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
            OrderModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            if (model.BuyerId != User.GetId())
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
            }

            model.ShouldBeDelivered = req.ShouldBeDelivered;
            await service.EditAsync(req.Id, model).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
