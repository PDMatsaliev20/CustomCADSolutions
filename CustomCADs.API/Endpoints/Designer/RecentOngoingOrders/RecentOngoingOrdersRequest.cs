using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.RecentOngoingOrders
{
    public class RecentOngoingOrdersRequest
    {
        [BindFrom("status")]
        public required string Status { get; set; }
        public int Limit { get; set; } = 4;
    }
}
