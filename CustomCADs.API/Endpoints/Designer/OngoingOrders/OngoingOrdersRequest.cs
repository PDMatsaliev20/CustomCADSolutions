namespace CustomCADs.API.Endpoints.Designer.OngoingOrders;

public class OngoingOrdersRequest
{
    public string? Status { get; set; } 
    public string? Sorting { get; set; }
    public string? Category { get; set; }
    public string? Name { get; set; }
    public string? Buyer { get; set; }
    public int Page { get; set; } = 1;
    public int Limit { get; set; } = 20;
}
