namespace CustomCADs.API.Endpoints.Payment.Purchase;

public class PurchaseRequest
{
    public int Id { get; set; }
    
    public required string PaymentMethodId { get; set; }
}
