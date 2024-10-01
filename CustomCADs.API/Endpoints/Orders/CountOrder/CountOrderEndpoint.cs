using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Orders;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.CountOrder
{
    using static StatusCodes;

    public class CountOrderEndpoint(IOrderService service) : EndpointWithoutRequest<OrderCountsResponse>
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
            bool predicate(OrderModel o, OrderStatus s)
                           => o.Status == s && o.Buyer.UserName == User.GetName();

            int pending = await service.CountAsync(o => predicate(o, OrderStatus.Pending)).ConfigureAwait(false);
            int begun = await service.CountAsync(o => predicate(o, OrderStatus.Begun)).ConfigureAwait(false);
            int finished = await service.CountAsync(o => predicate(o, OrderStatus.Finished)).ConfigureAwait(false);
            int reported = await service.CountAsync(o => predicate(o, OrderStatus.Reported)).ConfigureAwait(false);
            int removed = await service.CountAsync(o => predicate(o, OrderStatus.Removed)).ConfigureAwait(false);

            OrderCountsResponse response = new(pending, begun, finished, reported, removed);
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
