using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Queries.GetByUsername;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace CustomCADs.API.Endpoints.Identity.VerifyEmail;

using static ApiMessages;
using static StatusCodes;

public class VerifyEmailEndpoint(IMediator mediator, IAppUserManager manager, SignInManager<AppUser> signInManager, IConfiguration config) : Endpoint<VerifyEmailRequest>
{
    public override void Configure()
    {
        Get("VerifyEmail/{username}");
        Group<IdentityGroup>();
        Description(d => d
            .WithSummary("Checks the token's validity, and if successful verifies the user's email and  logs the him in.")
            .Produces<EmptyResponse>(Status200OK)
            .ProducesProblem(Status400BadRequest)
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(VerifyEmailRequest req, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(req.Token))
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(IsRequired, "Verify Email Token"),
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        AppUser? appUser = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
        if (appUser == null)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(NotFound, "Account"),
            });
            await SendErrorsAsync(Status404NotFound).ConfigureAwait(false);
            return;
        }

        if (appUser.EmailConfirmed)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = EmailAlreadyVerified,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        string decodedECT = req.Token.Replace(' ', '+');
        IdentityResult result = await manager.ConfirmEmailAsync(appUser, decodedECT).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = InvalidEmailToken,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        await signInManager.SignInAsync(appUser, false).ConfigureAwait(false);

        GetUserByUsernameQuery query = new(req.Username);
        UserModel model = await mediator.Send(query, ct).ConfigureAwait(false);

        HttpContext.Response.Cookies.Append("role", model.RoleName);
        HttpContext.Response.Cookies.Append("username", model.UserName);

        JwtSecurityToken jwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
        string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        HttpContext.Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo });

        (string newRT, DateTime newEnd) = await model.RenewRefreshToken(mediator, ct).ConfigureAwait(false);
        HttpContext.Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

        HttpContext.Response.Cookies.Append("role", model.RoleName, new() { Expires = newEnd });
        HttpContext.Response.Cookies.Append("username", model.UserName, new() { Expires = newEnd });

        await SendOkAsync("Welcome!").ConfigureAwait(false);
    }
}
