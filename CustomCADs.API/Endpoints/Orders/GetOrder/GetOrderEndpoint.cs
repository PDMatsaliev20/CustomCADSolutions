using AutoMapper;
using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Orders;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.GetOrder
{
    using static StatusCodes;

    public class GetOrderEndpoint(IOrderService service) : Endpoint<GetOrderRequest, OrderExportDTO>
    {
        public override void Configure()
        {
            Get("{id:int}");
            Group<OrdersGroup>();
            Description(d => d.WithSummary("Gets an Order by the specified Id."));
            Options(opt =>
            {
                opt.Produces<OrderExportDTO>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(GetOrderRequest req, CancellationToken ct)
        {
            OrderModel order = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            if (order.BuyerId != User.GetId())
            {
                await SendForbiddenAsync().ConfigureAwait(false);
                return;
            }

            OrderExportDTO result = new()
            {
                Id = order.Id,
                Name = order.Name,
                Description = order.Description,
                ShouldBeDelivered = order.ShouldBeDelivered,
                ImagePath = order.ImagePath,
                BuyerName = order.Buyer.UserName,
                OrderDate = order.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                Status = order.Status.ToString(),
                DesignerEmail = order.Designer?.Email,
                DesignerName = order.Designer?.UserName,
                CadId = order.CadId,
                Category = new()
                {
                    Id = order.CategoryId,
                    Name = order.Category.Name,
                },
            };
            await SendAsync(result, Status200OK).ConfigureAwait(false);
        }
    }
}
