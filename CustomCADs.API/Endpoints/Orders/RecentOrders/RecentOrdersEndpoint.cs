using AutoMapper;
using CustomCADs.API.Models.Orders;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using System.Collections.Generic;

namespace CustomCADs.API.Endpoints.Orders.RecentOrders
{
    using static StatusCodes;

    public class RecentOrdersEndpoint(IOrderService service) : Endpoint<RecentOrdersRequest, OrderResultDTO>
    {
        public override void Configure()
        {
            Get("Recent");
            Group<OrdersGroup>();
            Description(d => d.WithSummary("Gets the User's most recent Orders."));
            Options(opt =>
            {
                opt.Produces<OrderResultDTO>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(RecentOrdersRequest req, CancellationToken ct)
        {
            OrderResult result = service.GetAll(
                    buyer: User.Identity?.Name,
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
                        Description = o.Description,
                        ShouldBeDelivered = o.ShouldBeDelivered,
                        ImagePath = o.ImagePath,
                        BuyerName = o.Buyer.UserName,
                        OrderDate = o.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        Status = o.Status.ToString(),
                        DesignerEmail = o.Designer?.Email,
                        DesignerName = o.Designer?.UserName,
                        CadId = o.CadId,
                        Category = new() 
                        { 
                            Id = o.CategoryId,
                            Name = o.Category.Name,
                        },
                    }).ToArray(),
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
