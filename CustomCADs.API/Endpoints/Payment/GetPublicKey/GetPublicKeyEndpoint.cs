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
            Description(d => d
                .WithSummary("Gets the Public Key.")
                .Produces<string>(Status200OK));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string pk = service.GetPublicKey();
            await SendOkAsync(pk).ConfigureAwait(false);
        }
    }
}
