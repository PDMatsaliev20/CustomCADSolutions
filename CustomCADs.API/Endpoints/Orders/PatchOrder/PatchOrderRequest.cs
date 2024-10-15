namespace CustomCADs.API.Endpoints.Orders.PatchOrder;

public class PatchOrderRequest
{
    public int Id { get; set; }
    public bool ShouldBeDelivered { get; set; }
}
