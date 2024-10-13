using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrder
{
    using static StatusCodes;

    public class OngoingOrderEndpoint(IOrderService orderService) : Endpoint<OngoingOrderRequest, OngoingOrderResponse>
    {
        public override void Configure()
        {
            Get("Orders/{id:int}");
            Group<DesignerGroup>();
            Description(d => d
                .WithSummary("Gets the Order with the specified Id.")
                .Produces<OngoingOrderResponse>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(OngoingOrderRequest req, CancellationToken ct)
        {
            OrderModel model = await orderService.GetByIdAsync(req.Id).ConfigureAwait(false);
            
            OngoingOrderResponse response = model.Adapt<OngoingOrderResponse>();
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
