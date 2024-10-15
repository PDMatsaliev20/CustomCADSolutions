using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Commands.Delete;
using CustomCADs.Application.UseCases.Orders.Queries.GetById;
using CustomCADs.Application.UseCases.Orders.Queries.IsBuyer;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.DeleteOrder
{
    using static ApiMessages;
    using static StatusCodes;

    public class DeleteOrderEndpoint(IMediator mediator, IWebHostEnvironment env) : Endpoint<DeleteOrderRequest>
    {
        public override void Configure()
        {
            Delete("{id}");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Deletes the Order with the specified id.")
                .Produces<EmptyResponse>(Status204NoContent));
        }

        public override async Task HandleAsync(DeleteOrderRequest req, CancellationToken ct)
        {
            IsOrderBuyerQuery isBuyerQuery = new(req.Id, User.GetName());
            bool userIsBuyer = await mediator.Send(isBuyerQuery).ConfigureAwait(false);

            if (!userIsBuyer)
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            GetOrderByIdQuery getOrderQuery = new(req.Id);
            OrderModel model = await mediator.Send(getOrderQuery).ConfigureAwait(false);

            env.DeleteFile("orders", model.Name + model.Id, model.ImageExtension);

            DeleteOrderCommand command = new(req.Id);
            await mediator.Send(command).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
