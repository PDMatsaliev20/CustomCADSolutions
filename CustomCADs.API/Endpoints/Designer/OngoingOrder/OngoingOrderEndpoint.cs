using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Queries.GetById;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrder;

using static StatusCodes;

public class OngoingOrderEndpoint(IMediator mediator) : Endpoint<OngoingOrderRequest, OngoingOrderResponse>
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
        GetOrderByIdQuery query = new(req.Id);
        OrderModel model = await mediator.Send(query, ct).ConfigureAwait(false);
        
        OngoingOrderResponse response = model.Adapt<OngoingOrderResponse>();
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
