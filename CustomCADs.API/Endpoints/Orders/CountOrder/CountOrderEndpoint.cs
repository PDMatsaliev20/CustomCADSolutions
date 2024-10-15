using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Application.UseCases.Orders.Queries.Count;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.CountOrder
{
    using static StatusCodes;

    public class CountOrderEndpoint(IMediator mediator) : EndpointWithoutRequest<OrderCountsResponse>
    {
        public override void Configure()
        {
            Get("Counts");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Gets the counts of the User's Orders grouped by their status.")
                .Produces<OrderCountsResponse>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            OrdersCountQuery query;
            bool predicate(OrderModel o, OrderStatus s)
                           => o.Status == s && o.Buyer.UserName == User.GetName();

            query = new(o => predicate(o, OrderStatus.Pending));
            int pending = await mediator.Send(query);
            
            query = new(o => predicate(o, OrderStatus.Begun));
            int begun = await mediator.Send(query);

            query = new(o => predicate(o, OrderStatus.Finished));
            int finished = await mediator.Send(query).ConfigureAwait(false);
            
            query = new(o => predicate(o, OrderStatus.Reported));
            int reported = await mediator.Send(query).ConfigureAwait(false);
            
            query = new(o => predicate(o, OrderStatus.Removed));
            int removed = await mediator.Send(query).ConfigureAwait(false);

            OrderCountsResponse response = new(pending, begun, finished, reported, removed);
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
