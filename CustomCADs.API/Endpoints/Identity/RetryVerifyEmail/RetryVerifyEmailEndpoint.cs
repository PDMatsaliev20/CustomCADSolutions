using CustomCADs.Application.Contracts;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.RetryVerifyEmail
{
    using static ApiMessages;
    using static StatusCodes;

    public class RetryVerifyEmailEndpoint(IAppUserManager manager, IEmailService email, IConfiguration config) : Endpoint<RetryVerifyEmailRequest>
    {
        public override void Configure()
        {
            Get("RetryVerifyEmail");
            Group<IdentityGroup>();
            Description(d => d.WithSummary("Sends another email with a token."));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status200OK);
                opt.ProducesProblem(Status400BadRequest);
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(RetryVerifyEmailRequest req, CancellationToken ct)
        {
            AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            if (user == null)
            {
                await SendAsync(string.Format(NotFound, "Account"), Status404NotFound).ConfigureAwait(false);
                return;
            }

            if (user.EmailConfirmed)
            {
                await SendAsync(EmailAlreadyVerified, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            string serverUrl = config["URLs:Server"] ?? "https://customads.onrender.com";
            string token = await manager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

            string endpoint = Path.Combine(serverUrl, $"API/Identity/VerifyEmail/{req.Username}") + $"?token={token}";

            await email.SendVerificationEmailAsync(user.Email ?? "", endpoint).ConfigureAwait(false);
            await SendAsync("Check your email.", Status200OK).ConfigureAwait(false);
        }
    }
}
