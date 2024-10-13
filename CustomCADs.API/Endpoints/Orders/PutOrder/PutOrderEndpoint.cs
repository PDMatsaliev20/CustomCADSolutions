using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.PutOrder
{
    using static ApiMessages;
    using static StatusCodes;

    public class PutOrderEndpoint(IOrderService service) : Endpoint<PutOrderRequest>
    {
        public override void Configure()
        {
            Put("{id}");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Updates Name, Description and CategoryId for Orders have a Pending status.")
                .Accepts<PutOrderRequest>("multipart/form-data")
                .Produces<EmptyResponse>(Status204NoContent));
        }

        public override async Task HandleAsync(PutOrderRequest req, CancellationToken ct)
        {
            OrderModel order = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

            if (order.BuyerId != User.GetId())
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            if (order.Status != OrderStatus.Pending)
            {
                await SendErrorsAsync(Status400BadRequest).ConfigureAwait(false);
                return;
            }

            order.Name = req.Name;
            order.Description = req.Description;
            order.CategoryId = req.CategoryId;
            await service.EditAsync(req.Id, order).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
