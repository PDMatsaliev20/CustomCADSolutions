﻿using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Queries.GetByUsername;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace CustomCADs.API.Endpoints.Identity.Login;

using static ApiMessages;
using static StatusCodes;

public class LoginEndpoint(IMediator mediator, IAppUserManager manager, SignInManager<AppUser> signInManager, IConfiguration config) : Endpoint<LoginRequest>
{
    public override void Configure()
    {
        Post("Login");
        Group<IdentityGroup>();
        Description(d =>
            d.WithSummary("Logs into the account with the specified parameters.")
            .Produces<EmptyResponse>(Status200OK)
            .Produces(Status400BadRequest)
            .Produces(Status401Unauthorized)
            .Produces(Status423Locked));
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
        if (user == null || !user.EmailConfirmed)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = InvalidAccountOrEmail,
            });
            await SendErrorsAsync(Status401Unauthorized).ConfigureAwait(false);
            return;
        }

        if (await manager.IsLockedOutAsync(user).ConfigureAwait(false) && user.LockoutEnd.HasValue)
        {
            TimeSpan timeLeft = user.LockoutEnd.Value.Subtract(DateTimeOffset.UtcNow);
            await SendLockedOutAsync(timeLeft).ConfigureAwait(false);
            return;
        }


        SignInResult result = await signInManager.PasswordSignInAsync(user, req.Password,
            req.RememberMe, lockoutOnFailure: true).ConfigureAwait(false);

        if (!result.Succeeded)
        {
            if (result.IsLockedOut && user.LockoutEnd.HasValue)
            {
                TimeSpan timeLeft = user.LockoutEnd.Value.Subtract(DateTimeOffset.UtcNow);
                await SendLockedOutAsync(timeLeft).ConfigureAwait(false);
                return;
            }

            ValidationFailures.Add(new()
            {
                ErrorMessage = InvalidLogin,
            });
            await SendErrorsAsync(Status401Unauthorized).ConfigureAwait(false);
            return;
        }

        GetUserByUsernameQuery query = new(req.Username);
        UserModel model = await mediator.Send(query, ct).ConfigureAwait(false);

        JwtSecurityToken jwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);

        string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        CookieOptions jwtOptions = new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo };
        HttpContext.Response.Cookies.Append("jwt", signedJwt, jwtOptions);

        (string newRT, DateTime newEnd) = await model.RenewRefreshToken(mediator, ct).ConfigureAwait(false);
        CookieOptions rtOptions = new() { HttpOnly = true, Secure = true, Expires = newEnd };
        HttpContext.Response.Cookies.Append("rt", newRT, rtOptions);

        CookieOptions userInfoOptions = new() { Expires = newEnd };
        HttpContext.Response.Cookies.Append("role", model.RoleName, userInfoOptions);
        HttpContext.Response.Cookies.Append("username", model.UserName, userInfoOptions);

        await SendOkAsync("Welcome back!").ConfigureAwait(false);
    }

    private async Task SendLockedOutAsync(TimeSpan timeLeft)
    {
        var response = new
        {
            Seconds = Convert.ToInt16(timeLeft.TotalSeconds),
            Error = string.Format(LockedOutUser, Convert.ToInt16(timeLeft.TotalSeconds)),
        };

        await SendAsync(response, Status423Locked).ConfigureAwait(false);
    }
}
