﻿using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Queries.GetByRefreshToken;
using FastEndpoints;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace CustomCADs.API.Endpoints.Identity.RefreshToken;

using static ApiMessages;
using static StatusCodes;

public class RefreshTokenEndpoint(IMediator mediator, IConfiguration config) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Post("RefreshToken");
        Group<IdentityGroup>();
        Description(d => d
            .WithSummary("Returns a Refresh token")
            .Produces<EmptyResponse>(Status200OK));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string? rt = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "rt").Value;
        if (string.IsNullOrEmpty(rt))
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = NoRefreshToken,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        GetUserByRefreshTokenQuery query = new(rt);
        UserModel model = await mediator.Send(query, ct).ConfigureAwait(false);

        if (model.RefreshTokenEndDate < DateTime.UtcNow)
        {
            HttpContext.Response.Cookies.Delete("rt");
            HttpContext.Response.Cookies.Delete("username");
            HttpContext.Response.Cookies.Delete("userRole");

            ValidationFailures.Add(new() 
            {
                ErrorMessage = RefreshTokenExpired,
            });
            await SendErrorsAsync(Status401Unauthorized).ConfigureAwait(false);
            return;
        }
        JwtSecurityToken newJwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);

        string signedJwt = new JwtSecurityTokenHandler().WriteToken(newJwt);
        CookieOptions jwtOptions = new() { HttpOnly = true, Secure = true, Expires = newJwt.ValidTo };
        HttpContext.Response.Cookies.Append("jwt", signedJwt, jwtOptions);

        if (model.RefreshTokenEndDate >= DateTime.UtcNow.AddMinutes(1))
        {
            await SendOkAsync(NoNeedForNewRT).ConfigureAwait(false);
            return;
        }

        (string newRT, DateTime newEnd) = await model.RenewRefreshToken(mediator, ct).ConfigureAwait(false);
        CookieOptions rtOptions = new() { HttpOnly = true, Secure = true, Expires = newEnd };
        HttpContext.Response.Cookies.Append("rt", newRT, rtOptions);

        CookieOptions userInfoOptions = new() { Expires = newEnd };
        HttpContext.Response.Cookies.Append("role", model.RoleName, userInfoOptions);
        HttpContext.Response.Cookies.Append("username", model.UserName, userInfoOptions);

        await SendOkAsync(AccessTokenRenewed).ConfigureAwait(false);
    }
}
