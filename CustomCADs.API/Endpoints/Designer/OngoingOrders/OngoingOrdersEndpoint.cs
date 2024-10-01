using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrders
{
    using static ApiMessages;
    using static StatusCodes;

    public class OngoingOrdersEndpoint(IDesignerService service) : Endpoint<OngoingOrdersRequest, OrderResultDto<OngoingOrdersResponse>>
    {
        public override void Configure()
        {
            Get("Orders");
            Group<DesignerGroup>();
            Description(d => d
                .WithSummary("Gets all Orders with specified status.")
                .Produces<OrderResultDto<OngoingOrdersResponse>>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(OngoingOrdersRequest req, CancellationToken ct)
        {
            IEnumerable<string> statuses = Enum.GetNames<OrderStatus>().Select(s => s.ToLower());
            if (!statuses.Contains(req.Status.ToLower()))
            {
                IResult badReq = Results.BadRequest(string.Format(InvalidStatus, statuses));
                await SendResultAsync(badReq).ConfigureAwait(false);
                return;
            }

            OrderResult result = service.GetOrders(
                status: req.Status,
                category: req.Category,
                name: req.Name,
                buyer: req.Buyer,
                sorting: req.Sorting ?? "",
                page: req.Page,
                limit: req.Limit
            );

            OrderResultDto<OngoingOrdersResponse> response = new()
            {
                Count = result.Count,
                Orders = result.Orders.Select(order => order.Adapt<OngoingOrdersResponse>()).ToArray(),
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
