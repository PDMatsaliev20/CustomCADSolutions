using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Orders.GetOrders
{
    using static StatusCodes;

    public class GetOrdersEndpoint(IOrderService service) : Endpoint<GetOrdersRequest, OrderResultDto<GetOrdersResponse>>
    {
        public override void Configure()
        {
            Get("");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Queries the User's Orders with the specified parameters.")
                .Produces<OrderResultDto<GetOrdersResponse>>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(GetOrdersRequest req, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(req.Status) && !Enum.GetNames<OrderStatus>().Contains(req.Status))
            {
                await SendErrorsAsync(Status400BadRequest).ConfigureAwait(false);
                return;
            }

            OrderResult result = service.GetAll(
                    buyer: User.GetName(),
                    status: req.Status,
                    category: req.Category,
                    name: req.Name,
                    sorting: req.Sorting ?? "",
                    page: req.Page,
                    limit: req.Limit
                );

            OrderResultDto<GetOrdersResponse> response = new()
            {
                Count = result.Count,
                Orders = result.Orders.Select(order => order.Adapt<GetOrdersResponse>()).ToArray(),
            };

            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
