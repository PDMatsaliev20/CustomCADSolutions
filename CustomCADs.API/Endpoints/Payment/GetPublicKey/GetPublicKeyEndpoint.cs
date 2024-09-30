using CustomCADs.Application.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Payment.GetPublicKey
{
    using static StatusCodes;

    public class GetPublicKeyEndpoint(IPaymentService service) : EndpointWithoutRequest<string>
    {
        public override void Configure()
        {
            Get("GetPublicKey");
            Group<PaymentGroup>();
            Description(d => d.WithSummary("Gets the Public Key."));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string pk = service.GetPublicKey();
            await SendAsync(pk, Status200OK).ConfigureAwait(false);
        }
    }
}
