using CustomCADs.Application.Contracts;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.RetryVerifyEmail;

using static ApiMessages;
using static StatusCodes;

public class RetryVerifyEmailEndpoint(IAppUserManager manager, IEmailService email, IConfiguration config) : Endpoint<RetryVerifyEmailRequest>
{
    public override void Configure()
    {
        Get("RetryVerifyEmail/{username}");
        Group<IdentityGroup>();
        Description(d => d
            .WithSummary("Sends another email with a token.")
            .Produces<EmptyResponse>(Status200OK)
            .ProducesProblem(Status400BadRequest)
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(RetryVerifyEmailRequest req, CancellationToken ct)
    {
        AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
        if (user == null)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(NotFound, "Account"), 
            });
            await SendErrorsAsync(Status404NotFound).ConfigureAwait(false);
            return;
        }

        if (user.EmailConfirmed)
        {
            ValidationFailures.Add(new() 
            {
                ErrorMessage  = EmailAlreadyVerified,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        string serverUrl = config["URLs:Server"] ?? "https://customads.onrender.com";
        string token = await manager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

        string endpoint = Path.Combine(serverUrl, $"API/Identity/VerifyEmail/{req.Username}") + $"?token={token}";

        await email.SendVerificationEmailAsync(user.Email ?? "", endpoint).ConfigureAwait(false);
        await SendOkAsync("Check your email.").ConfigureAwait(false);
    }
}
