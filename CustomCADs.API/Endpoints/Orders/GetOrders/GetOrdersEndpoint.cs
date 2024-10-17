using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Queries.GetAll;
using CustomCADs.Domain.Orders.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.GetOrders;

using static StatusCodes;

public class GetOrdersEndpoint(IMediator mediator) : Endpoint<GetOrdersRequest, OrderResultDto<GetOrdersResponse>>
{
    public override void Configure()
    {
        Get("");
        Group<OrdersGroup>();
        Description(d => d
            .WithSummary("Queries the User's Orders with the specified parameters.")
            .Produces<OrderResultDto<GetOrdersResponse>>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(GetOrdersRequest req, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(req.Status) && !Enum.GetNames<OrderStatus>().Contains(req.Status))
        {
            await SendErrorsAsync(Status400BadRequest).ConfigureAwait(false);
            return;
        }

        GetAllOrdersQuery query = new(
            Buyer: User.GetName(),
            Status: req.Status,
            Category: req.Category,
            Name: req.Name,
            Sorting: req.Sorting ?? string.Empty,
            Page: req.Page,
            Limit: req.Limit
        );
        OrderResult result = await mediator.Send(query, ct).ConfigureAwait(false);

        OrderResultDto<GetOrdersResponse> response = new()
        {
            Count = result.Count,
            Orders = result.Orders.Adapt<ICollection<GetOrdersResponse>>(),
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
