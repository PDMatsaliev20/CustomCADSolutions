using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.RecentOngoingOrders
{
    using static StatusCodes;

    public class RecentOngoingOrdersEndpoint(IDesignerService service) : Endpoint<RecentOngoingOrdersRequest, OrderResultDTO>
    {
        public override void Configure()
        {
            Get("Orders/Recent");
            Group<DesignerGroup>();
            Description(d => d.WithSummary("Gets the User's most recent finished Orders."));
            Options(opt =>
            {
                opt.Produces<OrderExportDTO>(Status200OK);
            });
        }

        public override async Task HandleAsync(RecentOngoingOrdersRequest req, CancellationToken ct)
        {
            OrderResult result = service.GetOrders(
                status: req.Status,
                sorting: nameof(Sorting.Newest),
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
                        Status = o.Status.ToString(),
                        OrderDate = o.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        Category = new() 
                        {
                            Id = o.CategoryId,
                            Name = o.Category.Name, 
                        }
                    }).ToArray(),
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
