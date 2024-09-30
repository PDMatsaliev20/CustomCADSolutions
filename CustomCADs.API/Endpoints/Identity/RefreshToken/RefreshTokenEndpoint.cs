using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using FastEndpoints;
using System.IdentityModel.Tokens.Jwt;

namespace CustomCADs.API.Endpoints.Identity.RefreshToken
{
    using static ApiMessages;
    using static StatusCodes;

    public class RefreshTokenEndpoint(IUserService service, IConfiguration config) : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Post("RefreshToken");
            Group<IdentityGroup>();
            Description(d => d.WithSummary("Returns a Refresh token"));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status200OK);
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            string? rt = HttpContext.Request.Cookies.FirstOrDefault(c => c.Key == "rt").Value;
            if (string.IsNullOrEmpty(rt))
            {
                await SendAsync(NoRefreshToken, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            UserModel model = await service.GetByRefreshToken(rt).ConfigureAwait(false);
            if (model.RefreshTokenEndDate < DateTime.UtcNow)
            {
                HttpContext.Response.Cookies.Delete("rt");
                HttpContext.Response.Cookies.Delete("username");
                HttpContext.Response.Cookies.Delete("userRole");

                await SendAsync(RefreshTokenExpired, Status401Unauthorized).ConfigureAwait(false);
                return;
            }
            JwtSecurityToken newJwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);

            string signedJwt = new JwtSecurityTokenHandler().WriteToken(newJwt);
            CookieOptions jwtOptions = new() { HttpOnly = true, Secure = true, Expires = newJwt.ValidTo };
            HttpContext.Response.Cookies.Append("jwt", signedJwt, jwtOptions);

            if (model.RefreshTokenEndDate >= DateTime.UtcNow.AddMinutes(1))
            {
                await SendAsync(NoNeedForNewRT, Status200OK).ConfigureAwait(false);
                return;
            }

            (string newRT, DateTime newEnd) = await service.RenewRefreshToken(model).ConfigureAwait(false);
            CookieOptions rtOptions = new() { HttpOnly = true, Secure = true, Expires = newEnd };
            HttpContext.Response.Cookies.Append("rt", newRT, rtOptions);

            CookieOptions userInfoOptions = new() { Expires = newEnd };
            HttpContext.Response.Cookies.Append("role", model.RoleName, userInfoOptions);
            HttpContext.Response.Cookies.Append("username", model.UserName, userInfoOptions);

            await SendAsync(AccessTokenRenewed, Status200OK).ConfigureAwait(false);
        }
    }
}
