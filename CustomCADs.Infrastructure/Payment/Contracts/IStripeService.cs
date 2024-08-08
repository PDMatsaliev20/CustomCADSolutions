using CustomCADs.Infrastructure.Payment.DTOs;
using Stripe;

namespace CustomCADs.Infrastructure.Payment.Contracts
{
    public interface IStripeService
    {
        string GetPublicKey();
        Task<PaymentIntent> CapturePaymentAsync(string paymentIntentId);
        Task<PaymentIntent> ProcessPayment(string paymentMethod, PurchaseInfo purchase);
    }
}
