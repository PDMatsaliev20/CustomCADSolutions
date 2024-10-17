using CustomCADs.Application.Common.Dtos;

namespace CustomCADs.Application.Common.Contracts;

public interface IPaymentService
{
    string GetPublicKey();
    Task<PaymentResult> CapturePaymentAsync(string paymentIntentId);
    Task<PaymentResult> InitializePayment(string paymentMethod, PurchaseInfo purchase);
}
