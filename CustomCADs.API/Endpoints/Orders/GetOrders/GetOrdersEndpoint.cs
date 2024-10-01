using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Orders.GetOrders
{
    using static StatusCodes;

    public class GetOrdersEndpoint(IOrderService service) : Endpoint<GetOrdersRequest, OrderResultDto<GetOrdersResponse>>
    {
        public override void Configure()
        {
            Get("{status}");
            Group<OrdersGroup>();
            Description(d => d.WithSummary("Queries the User's Orders with the specified parameters."));
            Options(opt =>
            {
                opt.Produces<OrderResultDto<GetOrdersResponse>>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(GetOrdersRequest req, CancellationToken ct)
        {
            if (!Enum.GetNames<OrderStatus>().Contains(req.Status))
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
                Orders = result.Orders
                    .Select(o => new GetOrdersResponse()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Description = o.Description,
                        ShouldBeDelivered = o.ShouldBeDelivered,
                        ImagePath = o.ImagePath,
                        BuyerName = o.Buyer.UserName,
                        OrderDate = o.OrderDate.ToString(DateFormatString),
                        Status = o.Status.ToString(),
                        DesignerEmail = o.Designer?.Email,
                        DesignerName = o.Designer?.UserName,
                        CadId = o.CadId,
                        Category = new(o.CategoryId, o.Category.Name),
                    }).ToArray(),
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
