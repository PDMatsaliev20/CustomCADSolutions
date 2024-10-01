using CustomCADs.API.Helpers;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.Authentication
{
    using static StatusCodes;

    public class AuthenticationEndpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Get("Authentication");
            Group<IdentityGroup>();
            Description(d => d
                .WithSummary("Gets info about User Authentication.")
                .Produces<EmptyResponse>(Status200OK));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await SendAsync(User.GetAuth(), Status200OK).ConfigureAwait(false);
        }
    }
}
