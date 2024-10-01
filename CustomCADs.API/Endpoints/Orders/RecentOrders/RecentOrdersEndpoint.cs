using CustomCADs.API.Helpers;
using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using static CustomCADs.Domain.DataConstants;
using Mapster;

namespace CustomCADs.API.Endpoints.Orders.RecentOrders
{
    using static StatusCodes;

    public class RecentOrdersEndpoint(IOrderService service) : Endpoint<RecentOrdersRequest, OrderResultDto<RecentOrdersResponse>>
    {
        public override void Configure()
        {
            Get("Recent");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Gets the User's most recent Orders.")
                .Produces<OrderResultDto<RecentOrdersResponse>>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(RecentOrdersRequest req, CancellationToken ct)
        {
            OrderResult result = service.GetAll(
                buyer: User.GetName(),
                sorting: nameof(Sorting.Newest),
                limit: req.Limit
            );

            OrderResultDto<RecentOrdersResponse> response = new()
            {
                Count = result.Count,
                Orders = result.Orders.Select(order => order.Adapt<RecentOrdersResponse>()).ToArray(),
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
