using AutoMapper;
using CustomCADs.API.Models.Orders;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrder
{
    using static StatusCodes;

    public class OngoingOrderEndpoint(IDesignerService service) : Endpoint<OngoingOrderRequest, OrderExportDTO>
    {
        public override void Configure()
        {
            Get("Orders/{id}");
            Group<DesignerGroup>();
            Description(d => d.WithSummary("Gets the Order with the specified Id."));
            Options(opt =>
            {
                opt.Produces<OrderExportDTO>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(OngoingOrderRequest req, CancellationToken ct)
        {
            OrderResult result = service.GetOrders(id: req.Id);
            if (result.Count != 1)
            {
                await SendErrorsAsync(result.Count == 0 ? Status404NotFound : Status500InternalServerError).ConfigureAwait(false);
                return;
            }

            OrderModel model = result.Orders.Single();
            OrderExportDTO response = new()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Status = model.Status.ToString(),
                OrderDate = model.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                BuyerName = model.Buyer.UserName,
                Category = new()
                {
                    Id = model.CategoryId,
                    Name = model.Category.Name,
                }
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
