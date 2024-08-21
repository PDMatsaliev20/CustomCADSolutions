namespace CustomCADs.Application.DTOs.Payment
{
    public class PaymentResult
    {
        public string Id { get; set; } = null!;
        public string Status { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
    }
}
