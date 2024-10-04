﻿using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Identity.ResetPassword
{
    using static ApiMessages;
    using static StatusCodes;

    public class ResetPasswordEndpoint(IAppUserManager manager) : Endpoint<ResetPasswordRequest>
    {
        public override void Configure()
        {
            Post("ResetPassword/{email}");
            Group<IdentityGroup>();
            Description(d => d
                .WithSummary("Resets password of User with given email if the token is valid")
                .Produces<EmptyResponse>(Status200OK));
        }

        public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(req.Token))
            {
                object details = new { error = "Bad Request", message = string.Format(IsRequired, "Reset Password Token") };
                await SendResultAsync(Results.BadRequest(details)).ConfigureAwait(false);
                return;
            }
            
            if (string.IsNullOrEmpty(req.NewPassword))
            {
                object details = new { error = "Bad Request", message = string.Format(IsRequired, "New Password") };
                await SendResultAsync(Results.BadRequest(details)).ConfigureAwait(false);
                return;
            }

            AppUser? user = await manager.FindByEmailAsync(req.Email).ConfigureAwait(false);
            if (user == null)
            {
                await SendAsync(string.Format(NotFound, "User"), Status404NotFound);
                return;
            }

            string encodedToken = req.Token.Replace(' ', '+');
            IdentityResult result = await manager.ResetPasswordAsync(user, encodedToken, req.NewPassword).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                await SendAsync(result.Errors, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            await SendAsync("Done!", Status200OK).ConfigureAwait(false);
        }
    }
}
