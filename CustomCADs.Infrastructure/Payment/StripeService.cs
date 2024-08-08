using Microsoft.Extensions.Options;
using Stripe;

namespace CustomCADs.Infrastructure.Payment
{
    public class StripeService(IOptions<StripeKeys> options, PaymentIntentService paymentIntentService) : IStripeService
    {
        public readonly StripeKeys keys = options.Value;

        public string GetPublicKey() => keys.TestPublishableKey;

        public async Task<PaymentIntent> CapturePaymentAsync(string paymentIntentId)
            => await paymentIntentService.CaptureAsync(paymentIntentId).ConfigureAwait(false);

        public async Task<PaymentIntent> ProcessPayment(string paymentMethodId, PurchaseInfo purchase)
        {
            StripeConfiguration.ApiKey = keys.TestSecretKey;
            
            return await paymentIntentService.CreateAsync(new()
            {
                Amount = Convert.ToInt64(purchase.Price * 100),
                Currency = "USD",
                PaymentMethod = paymentMethodId,
                Confirm = true,
                Description = $"{purchase.Buyer} bought {purchase.Seller}'s {purchase.Product} for {purchase.Price}$.",
                AutomaticPaymentMethods = new()
                {
                    Enabled = true,
                    AllowRedirects = "never"
                }
            }).ConfigureAwait(false);
        }
    }
}
