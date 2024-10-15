using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Queries.GetAll;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.RecentOrders;

using static StatusCodes;

public class RecentOrdersEndpoint(IMediator mediator) : Endpoint<RecentOrdersRequest, IEnumerable<RecentOrdersResponse>>
{
    public override void Configure()
    {
        Get("Recent");
        Group<OrdersGroup>();
        Description(d => d
            .WithSummary("Gets the User's most recent Orders.")
            .Produces<IEnumerable<RecentOrdersResponse>>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(RecentOrdersRequest req, CancellationToken ct)
    {
        GetAllOrdersQuery query = new(
            Buyer: User.GetName(),
            Sorting: nameof(Sorting.Newest),
            Limit: req.Limit
        );
        OrderResult result = await mediator.Send(query).ConfigureAwait(false);
        
        var response = result.Orders.Select(order => order.Adapt<RecentOrdersResponse>());
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
