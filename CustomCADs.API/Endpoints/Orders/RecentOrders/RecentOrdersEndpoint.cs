using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Orders.RecentOrders
{
    using static StatusCodes;

    public class RecentOrdersEndpoint(IOrderService service) : Endpoint<RecentOrdersRequest, IEnumerable<RecentOrdersResponse>>
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
            OrderResult result = service.GetAll(
                buyer: User.GetName(),
                sorting: nameof(Sorting.Newest),
                limit: req.Limit
            );

            var response = result.Orders.Select(order => order.Adapt<RecentOrdersResponse>());
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
