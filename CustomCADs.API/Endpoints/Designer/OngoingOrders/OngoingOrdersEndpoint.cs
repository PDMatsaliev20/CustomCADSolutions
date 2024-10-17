using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Queries.GetAll;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.API.Endpoints.Designer.OngoingOrders;

using static ApiMessages;
using static StatusCodes;

public class OngoingOrdersEndpoint(IMediator mediator) : Endpoint<OngoingOrdersRequest, OrderResultDto<OngoingOrdersResponse>>
{
    public override void Configure()
    {
        Get("Orders");
        Group<DesignerGroup>();
        Description(d => d
            .WithSummary("Gets all Orders with specified status.")
            .Produces<OrderResultDto<OngoingOrdersResponse>>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(OngoingOrdersRequest req, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(req.Status))
        {
            IEnumerable<string> statuses = Enum.GetNames<OrderStatus>().Select(s => s.ToLower());
            if (!statuses.Contains(req.Status.ToLower()))
            {
                ValidationFailures.Add(new()
                {
                    PropertyName = nameof(req.Status),
                    AttemptedValue = req.Status,
                    ErrorMessage = string.Format(InvalidStatus, statuses)
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }
        }

        GetAllOrdersQuery query = new(
            Status: req.Status,
            Category: req.Category,
            Name: req.Name,
            Buyer: req.Buyer,
            Sorting: req.Sorting ?? "",
            Page: req.Page,
            Limit: req.Limit
        );
        OrderResult result = await mediator.Send(query, ct).ConfigureAwait(false);

        OrderResultDto<OngoingOrdersResponse> response = new()
        {
            Count = result.Count,
            Orders = result.Orders.Select(order => order.Adapt<OngoingOrdersResponse>()).ToArray(),
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
