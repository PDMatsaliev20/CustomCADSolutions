using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.DTOs.Payment;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Payment;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Payment.Purchase
{
    using static ApiMessages;
    using static StatusCodes;

    public class PurchaseEndpoint(IPaymentService service, ICadService cadService) : Endpoint<PurchaseRequest, string>
    {
        public override void Configure()
        {
            Post("{id}/Purchase");
            Group<PaymentGroup>();
            Description(d => d
                .WithSummary("Initializes the Payment Intent and returns a Client Secret if an error occurs.")
                .Produces<string>(Status200OK)
                .ProducesProblem(Status400BadRequest));
        }

        public override async Task HandleAsync(PurchaseRequest req, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(req.PaymentMethodId))
            {
                object details = new { error = "Bad Request", message = string.Format(IsRequired, "Payment Method Id") };
                await SendResultAsync(Results.BadRequest(details)).ConfigureAwait(false);
                return;
            }

            CadModel cad = await cadService.GetByIdAsync(req.Id).ConfigureAwait(false);

            PurchaseInfo purchase = new()
            {
                Product = cad.Name,
                Price = cad.Price,
                Seller = cad.Creator.UserName,
                Buyer = User.GetName(),
            };
            PaymentResult paymentIntent = await service.InitializePayment(req.PaymentMethodId, purchase).ConfigureAwait(false);

            string message = await CheckStatus(paymentIntent.Status).ConfigureAwait(false);

            string[] errorMessages = [CanceledPayment, FailedPaymentMethod, FailedPayment, FailedPaymentCapture, string.Format(UnhandledPayment, paymentIntent.Status)];
            if (!errorMessages.Contains(message))
            {
                await SendAsync(message, Status200OK).ConfigureAwait(false);
                return;
            }

            if (message != FailedPayment)
            {
                await SendAsync(message, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            string retry = await CheckStatus(paymentIntent.Status, paymentIntent.Id).ConfigureAwait(false);
            if (retry != SuccessfulPayment)
            {
                await SendAsync(paymentIntent.ClientSecret, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            await SendAsync(message, Status200OK).ConfigureAwait(false);
        }

        private async Task<string> CheckStatus(string status, string? id = null)
            => status switch
            {
                "succeeded" => SuccessfulPayment,
                "processing" => ProcessingPayment,
                "canceled" => CanceledPayment,
                "requires_payment_method" => FailedPaymentMethod,
                "requires_action" => FailedPayment,
                "requires_capture" => string.IsNullOrEmpty(id)
                    ? FailedPaymentCapture
                    : (await service.CapturePaymentAsync(id).ConfigureAwait(false)).Status == "succeeded"
                        ? SuccessfulPayment
                        : FailedPaymentCapture,
                _ => string.Format(UnhandledPayment, status)
            };
    }
}
