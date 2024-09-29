using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.GetOrders
{
    public class GetOrdersRequest
    {
        [BindFrom("status")]
        public required string Status { get; set; }
        
        [QueryParam]
        public string? Sorting { get; set; }
        
        [QueryParam]
        public string? Category { get; set; }
        
        [QueryParam]
        public string? Name { get; set; }
        
        [QueryParam]
        public int Page { get; set; } = 1;
        
        [QueryParam]
        public int Limit { get; set; } = 20;
    }
}
