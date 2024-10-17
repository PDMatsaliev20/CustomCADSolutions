namespace CustomCADs.Application.Common.Dtos;

public class PaymentResult
{
    public required string Id { get; set; }
    public required string Status { get; set; }
    public required string ClientSecret { get; set; }
}
