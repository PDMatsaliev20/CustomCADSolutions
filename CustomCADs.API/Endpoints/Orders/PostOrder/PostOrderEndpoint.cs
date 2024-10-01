using CustomCADs.API.Endpoints.Orders.GetOrder;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.PostOrder
{
    using static StatusCodes;

    public class PostOrderEndpoint(IOrderService service, IWebHostEnvironment env) : Endpoint<PostOrderRequest, PostOrderResponse>
    {
        public override void Configure()
        {
            Post("");
            Group<OrdersGroup>();
            Description(d => d.WithSummary("Creates an Order entity in the database."));
            Options(opt =>
            {
                opt.Accepts<PostOrderRequest>("multipart/form-data");
                opt.Produces<PostOrderResponse>(Status201Created, "application/json");
            });
            AllowFormData();
            AllowFileUploads();
        }

        public override async Task HandleAsync(PostOrderRequest req, CancellationToken ct)
        {
            OrderModel model = new()
            {
                Name = req.Name,
                Description = req.Description,
                CategoryId = req.CategoryId,
                ShouldBeDelivered = req.ShouldBeDelivered,
                OrderDate = DateTime.Now,
                BuyerId = User.GetId(),
                ImagePath = string.Empty,
            };
            int id = await service.CreateAsync(model).ConfigureAwait(false);

            string imagePath = await env.UploadOrderAsync(req.Image, req.Name + id + req.Image.GetFileExtension()).ConfigureAwait(false);
            await service.SetImagePathAsync(id, imagePath).ConfigureAwait(false);

            OrderModel createdOrder = await service.GetByIdAsync(id).ConfigureAwait(false);

            PostOrderResponse result = new()
            {
                Id = createdOrder.Id,
                Name = createdOrder.Name,
                Description = createdOrder.Description,
                ShouldBeDelivered = createdOrder.ShouldBeDelivered,
                ImagePath = imagePath,
                BuyerName = createdOrder.Buyer.UserName,
                OrderDate = createdOrder.OrderDate.ToString("dd-MM-yyyy HH:mm:ss"),
                Status = createdOrder.Status.ToString(),
                Category = new(createdOrder.CategoryId, createdOrder.Category.Name),
            };
            await SendCreatedAtAsync<GetOrderEndpoint>(new { id }, result);
        }
    }
}
