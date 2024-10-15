namespace CustomCADs.API.Endpoints.Orders.PostOrder;

public class PostOrderRequest
{
    public required string Name { get; set; } 
    public required string Description { get; set; } 
    public int CategoryId { get; set; }
    public bool ShouldBeDelivered { get; set; } = false;
    public required IFormFile Image { get; set; } 
}
