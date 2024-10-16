using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Queries.GetAll;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Designer.RecentOngoingOrders;

using static StatusCodes;

public class RecentOngoingOrdersEndpoint(IMediator mediator) : Endpoint<RecentOngoingOrdersRequest, IEnumerable<RecentOngoingOrdersResponse>>
{
    public override void Configure()
    {
        Get("Orders/Recent");
        Group<DesignerGroup>();
        Description(d => d
            .WithSummary("Gets the User's most recent finished Orders.")
            .Produces<IEnumerable<RecentOngoingOrdersResponse>>(Status200OK));
    }

    public override async Task HandleAsync(RecentOngoingOrdersRequest req, CancellationToken ct)
    {
        GetAllOrdersQuery query = new(
            Status: req.Status, 
            Sorting: nameof(Sorting.Newest), 
            Limit: req.Limit
        );
        OrderResult result = await mediator.Send(query).ConfigureAwait(false);

        var response = result.Orders.Select(order => order.Adapt<RecentOngoingOrdersResponse>());
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
