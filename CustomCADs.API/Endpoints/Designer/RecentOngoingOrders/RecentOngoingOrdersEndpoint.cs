using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Designer.RecentOngoingOrders
{
    using static StatusCodes;

    public class RecentOngoingOrdersEndpoint(IDesignerService service) : Endpoint<RecentOngoingOrdersRequest, OrderResultDto<RecentOngoingOrdersResponse>>
    {
        public override void Configure()
        {
            Get("Orders/Recent");
            Group<DesignerGroup>();
            Description(d => d
                .WithSummary("Gets the User's most recent finished Orders.")
                .Produces<OrderResultDto<RecentOngoingOrdersResponse>>(Status200OK));
        }

        public override async Task HandleAsync(RecentOngoingOrdersRequest req, CancellationToken ct)
        {
            OrderResult result = service.GetOrders(
                status: req.Status,
                sorting: nameof(Sorting.Newest),
                limit: req.Limit
            );

            OrderResultDto<RecentOngoingOrdersResponse> response = new()
            {
                Count = result.Count,
                Orders = result.Orders
                    .Select(o => new RecentOngoingOrdersResponse()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Status = o.Status.ToString(),
                        OrderDate = o.OrderDate.ToString(DateFormatString),
                        Category = new(o.CategoryId, o.Category.Name),
                    }).ToArray(),
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
