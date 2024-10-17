using CustomCADs.API.Endpoints.Orders.GetOrder;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Commands.Create;
using CustomCADs.Application.UseCases.Orders.Commands.SetImagePath;
using CustomCADs.Application.UseCases.Orders.Queries.GetById;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.PostOrder;

using static StatusCodes;

public class PostOrderEndpoint(IMediator mediator, IWebHostEnvironment env) : Endpoint<PostOrderRequest, PostOrderResponse>
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

        CreateOrderCommand createCommand = new(model);
        int id = await mediator.Send(createCommand, ct).ConfigureAwait(false);

        string imagePath = await env.UploadOrderAsync(req.Image, req.Name + id + req.Image.GetFileExtension()).ConfigureAwait(false);
        
        SetOrderImagePathCommand setImagePathCommand = new(id, imagePath);
        await mediator.Send(setImagePathCommand, ct).ConfigureAwait(false);

        GetOrderByIdQuery query = new(id);
        OrderModel createdOrder = await mediator.Send(query, ct).ConfigureAwait(false);

        PostOrderResponse response = createdOrder.Adapt<PostOrderResponse>();
        await SendCreatedAtAsync<GetOrderEndpoint>(new { id }, response);
    }
}
