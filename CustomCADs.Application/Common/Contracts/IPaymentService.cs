using CustomCADs.Application.Common.Dtos;
using CustomCADs.Application.Models.Payment;

namespace CustomCADs.Application.Common.Contracts;

public interface IPaymentService
{
    string GetPublicKey();
    Task<PaymentResult> CapturePaymentAsync(string paymentIntentId);
    Task<PaymentResult> InitializePayment(string paymentMethod, PurchaseInfo purchase);
}
