using CustomCADs.Infrastructure.Identity.Managers;
using CustomCADs.Infrastructure.Identity;
using FastEndpoints;
using CustomCADs.Infrastructure.Identity.Contracts;

namespace CustomCADs.API.Endpoints.Identity.EmailConfirmed
{
    using static StatusCodes;

    public class EmailConfirmedEndpoint(IAppUserManager manager) : Endpoint<EmailConfirmedRequest>
    {
        public override void Configure()
        {
            Get("IsEmailConfirmed/{username}");
            Group<IdentityGroup>();
            Description(d => d.WithSummary("Gets info about User Email Status"));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status200OK);
            });
        }

        public override async Task HandleAsync(EmailConfirmedRequest req, CancellationToken ct)
        {
            AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            bool isEmailConfirmed = user?.EmailConfirmed ?? false;

            await SendAsync(isEmailConfirmed, Status200OK).ConfigureAwait(false);
        }
    }
}
