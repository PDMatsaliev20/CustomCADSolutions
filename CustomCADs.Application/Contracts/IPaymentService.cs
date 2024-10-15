using CustomCADs.Application.DTOs.Payment;
using CustomCADs.Application.Models.Payment;

namespace CustomCADs.Application.Contracts;

public interface IPaymentService
{
    string GetPublicKey();
    Task<PaymentResult> CapturePaymentAsync(string paymentIntentId);
    Task<PaymentResult> InitializePayment(string paymentMethod, PurchaseInfo purchase);
}
