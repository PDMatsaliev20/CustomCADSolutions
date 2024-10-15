using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Commands.Edit;
using CustomCADs.Application.UseCases.Orders.Queries.GetById;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.PutOrder
{
    using static ApiMessages;
    using static StatusCodes;

    public class PutOrderEndpoint(IMediator mediator) : Endpoint<PutOrderRequest>
    {
        public override void Configure()
        {
            Put("{id}");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Updates Name, Description and CategoryId for Orders have a Pending status.")
                .Accepts<PutOrderRequest>("multipart/form-data")
                .Produces<EmptyResponse>(Status204NoContent));
        }

        public override async Task HandleAsync(PutOrderRequest req, CancellationToken ct)
        {
            GetOrderByIdQuery query = new(req.Id);
            OrderModel order = await mediator.Send(query).ConfigureAwait(false);

            if (order.BuyerId != User.GetId())
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            if (order.Status != OrderStatus.Pending)
            {
                await SendErrorsAsync(Status400BadRequest).ConfigureAwait(false);
                return;
            }

            order.Name = req.Name;
            order.Description = req.Description;
            order.CategoryId = req.CategoryId;

            EditOrderCommand command = new(req.Id, order);
            await mediator.Send(command).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
