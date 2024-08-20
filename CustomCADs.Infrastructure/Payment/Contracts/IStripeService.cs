using CustomCADs.Infrastructure.Payment.DTOs;

namespace CustomCADs.Infrastructure.Payment.Contracts
{
    public interface IStripeService
    {
        string GetPublicKey();
        Task<PaymentResult> CapturePaymentAsync(string paymentIntentId);
        Task<PaymentResult> ProcessPayment(string paymentMethod, PurchaseInfo purchase);
    }
}
