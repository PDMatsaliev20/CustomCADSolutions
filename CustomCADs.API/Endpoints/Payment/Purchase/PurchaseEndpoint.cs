using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.DTOs.Payment;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Models.Payment;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Payment.Purchase
{
    using static ApiMessages;
    using static StatusCodes;

    public class PurchaseEndpoint(IMediator mediator, IPaymentService service) : Endpoint<PurchaseRequest, string>
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
            GetCadByIdQuery query = new(req.Id);
            CadModel cad = await mediator.Send(query).ConfigureAwait(false);

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
                await SendOkAsync(message).ConfigureAwait(false);
                return;
            }

            if (message != FailedPayment)
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = message,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            string retry = await CheckStatus(paymentIntent.Status, paymentIntent.Id).ConfigureAwait(false);
            if (retry != SuccessfulPayment)
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = message,
                });
                await SendAsync(paymentIntent.ClientSecret, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            await SendOkAsync(message).ConfigureAwait(false);
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
