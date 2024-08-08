using Stripe;

namespace CustomCADs.Infrastructure.Payment
{
    public interface IStripeService
    {
        string GetPublicKey();
        Task<PaymentIntent> CapturePaymentAsync(string paymentIntentId);
        Task<PaymentIntent> ProcessPayment(string paymentMethod, PurchaseInfo purchase);
    }
}
