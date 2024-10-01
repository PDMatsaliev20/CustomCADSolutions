using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.DeleteOrder
{
    using static StatusCodes;

    public class DeleteOrderEndpoint(IOrderService service, IWebHostEnvironment env) : Endpoint<DeleteOrderRequest>
    {
        public override void Configure()
        {
            Delete("{id}");
            Group<OrdersGroup>();
            Description(d => d.WithSummary("Deletes the Order with the specified id."));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status204NoContent);
            });
        }

        public override async Task HandleAsync(DeleteOrderRequest req, CancellationToken ct)
        {
            OrderModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            if (model.Buyer.UserName != User.GetName())
            {
                await SendForbiddenAsync().ConfigureAwait(false);
                return;
            }

            if (string.IsNullOrEmpty(model.ImagePath))
            {
                env.DeleteFile("orders", model.Name + model.Id, model.ImageExtension!);
            }
            await service.DeleteAsync(req.Id).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
