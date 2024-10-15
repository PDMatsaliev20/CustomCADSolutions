using CustomCADs.API.Endpoints.Orders.GetOrder;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using CustomCADs.Application.UseCases.Orders.Commands.Create;
using CustomCADs.Application.UseCases.Orders.Queries.GetById;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.GalleryOrder;

using static ApiMessages;
using static StatusCodes;

public class GalleryOrderEndpoint(IMediator mediator) : Endpoint<GalleryOrderRequest, GalleryOrderResponse>
{
    public override void Configure()
    {
        Post("{cadId}");
        Group<OrdersGroup>();
        Description(d => d
            .WithSummary("Creates an Order entity with a Relation to the Cad with the specified id in the database.")
            .Produces<GalleryOrderResponse>(Status201Created, "application/json"));
    }

    public override async Task HandleAsync(GalleryOrderRequest req, CancellationToken ct)
    {
        GetCadByIdQuery query = new(req.CadId);
        CadModel cad = await mediator.Send(query).ConfigureAwait(false);

        OrderModel order = new()
        {
            Name = cad.Name,
            Description = string.Format(CadPurchasedMessage, req.CadId),
            Status = OrderStatus.Finished,
            CategoryId = cad.CategoryId,
            OrderDate = DateTime.Now,
            CadId = req.CadId,
            BuyerId = User.GetId(),
            DesignerId = cad.CreatorId,
            ImagePath = cad.Paths.ImagePath,
        };

        CreateOrderCommand command = new(order);
        int id = await mediator.Send(command).ConfigureAwait(false);

        GetOrderByIdQuery orderByIdQuery = new(id);
        OrderModel createdOrder = await mediator.Send(orderByIdQuery).ConfigureAwait(false);

        GalleryOrderResponse response = createdOrder.Adapt<GalleryOrderResponse>();
        await SendCreatedAtAsync<GetOrderEndpoint>(new { id }, response).ConfigureAwait(false);
    }
}
