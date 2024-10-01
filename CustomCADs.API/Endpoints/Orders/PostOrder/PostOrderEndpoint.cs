using CustomCADs.API.Endpoints.Orders.GetOrder;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Orders.PostOrder
{
    using static StatusCodes;

    public class PostOrderEndpoint(IOrderService service, IWebHostEnvironment env) : Endpoint<PostOrderRequest, PostOrderResponse>
    {
        public override void Configure()
        {
            Post("");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Creates an Order entity in the database.")
                .Accepts<PostOrderRequest>("multipart/form-data")
                .Produces<PostOrderResponse>(Status201Created, "application/json"));
        }

        public override async Task HandleAsync(PostOrderRequest req, CancellationToken ct)
        {
            OrderModel model = req.Adapt<OrderModel>();
            model.OrderDate = DateTime.Now;
            model.BuyerId = User.GetId();
            model.ImagePath = string.Empty;

            int id = await service.CreateAsync(model).ConfigureAwait(false);

            string imagePath = await env.UploadOrderAsync(req.Image, req.Name + id + req.Image.GetFileExtension()).ConfigureAwait(false);
            await service.SetImagePathAsync(id, imagePath).ConfigureAwait(false);

            OrderModel createdOrder = await service.GetByIdAsync(id).ConfigureAwait(false);

            PostOrderResponse response = createdOrder.Adapt<PostOrderResponse>();
            await SendCreatedAtAsync<GetOrderEndpoint>(new { id }, response);
        }
    }
}
