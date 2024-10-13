using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Designer.RecentOngoingOrders
{
    using static StatusCodes;

    public class RecentOngoingOrdersEndpoint(IDesignerService service) : Endpoint<RecentOngoingOrdersRequest, IEnumerable<RecentOngoingOrdersResponse>>
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
            OrderResult result = service.GetOrders(
                status: req.Status,
                sorting: nameof(Sorting.Newest),
                limit: req.Limit
            );

            var response = result.Orders.Select(order => order.Adapt<RecentOngoingOrdersResponse>());
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
