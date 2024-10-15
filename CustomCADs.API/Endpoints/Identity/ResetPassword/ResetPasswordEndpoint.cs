using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Identity.ResetPassword;

using static ApiMessages;
using static StatusCodes;

public class ResetPasswordEndpoint(IAppUserManager manager) : Endpoint<ResetPasswordRequest>
{
    public override void Configure()
    {
        Post("ResetPassword");
        Group<IdentityGroup>();
        Description(d => d
            .WithSummary("Resets password of User with given email if the token is valid")
            .Produces<EmptyResponse>(Status200OK));
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        AppUser? user = await manager.FindByEmailAsync(req.Email).ConfigureAwait(false);
        if (user == null)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(NotFound, "User"),
            });
            await SendErrorsAsync(Status404NotFound);
            return;
        }

        string encodedToken = req.Token.Replace(' ', '+');
        IdentityResult result = await manager.ResetPasswordAsync(user, encodedToken, req.NewPassword).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            var failures = result.Errors.Select(e => new ValidationFailure()
            {
                ErrorMessage = e.Description
            });
            ValidationFailures.AddRange(failures);

            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        await SendOkAsync("Done!").ConfigureAwait(false);
    }
}
