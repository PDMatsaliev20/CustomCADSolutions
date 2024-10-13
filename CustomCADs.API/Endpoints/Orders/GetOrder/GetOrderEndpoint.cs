using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Orders.GetOrder
{
    using static ApiMessages;
    using static StatusCodes;

    public class GetOrderEndpoint(IOrderService service) : Endpoint<GetOrderRequest, GetOrderResponse>
    {
        public override void Configure()
        {
            Get("{id:int}");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Gets an Order by the specified Id.")
                .Produces<GetOrderResponse>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(GetOrderRequest req, CancellationToken ct)
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

            GetOrderResponse response = order.Adapt<GetOrderResponse>();
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
