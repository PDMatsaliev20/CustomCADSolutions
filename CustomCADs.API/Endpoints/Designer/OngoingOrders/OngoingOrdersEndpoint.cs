using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrders
{
    using static ApiMessages;
    using static StatusCodes;

    public class OngoingOrdersEndpoint(IDesignerService service) : Endpoint<OngoingOrdersRequest, OrderResultDTO>
    {
        public override void Configure()
        {
            Get("Orders");
            Group<DesignerGroup>();
            Description(d => d.WithSummary("Gets all Orders with specified status."));
            Options(opt =>
            {
                opt.Produces<OrderResultDTO>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(OngoingOrdersRequest req, CancellationToken ct)
        {
            string[] statuses = Enum.GetNames<OrderStatus>();
            if (!statuses.Contains(req.Status.Capitalize()))
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

            OrderResultDTO response = new()
            {
                Count = result.Count,
                Orders = result.Orders
                    .Select(o => new OrderExportDTO()
                    {
                        Id = o.Id,
                        Name = o.Name,
                        Description = o.Description,
                        ImagePath = o.ImagePath,
                        OrderDate = o.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        ShouldBeDelivered = o.ShouldBeDelivered,
                        Category = new()
                        {
                            Id = o.CategoryId,
                            Name = o.Category.Name,
                        },
                    }).ToArray()
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
