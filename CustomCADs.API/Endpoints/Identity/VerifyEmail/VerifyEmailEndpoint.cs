using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;

namespace CustomCADs.API.Endpoints.Identity.VerifyEmail
{
    using static ApiMessages;
    using static StatusCodes;

    public class VerifyEmailEndpoint(IAppUserManager manager, IUserService service, SignInManager<AppUser> signInManager, IConfiguration config) : Endpoint<VerifyEmailRequest>
    {
        public override void Configure()
        {
            Get("VerifyEmail/{username}");
            Group<IdentityGroup>();
            Description(d => d.WithSummary("Checks the token's validity, and if successful verifies the user's email and  logs the him in."));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status200OK);
                opt.ProducesProblem(Status400BadRequest);
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(VerifyEmailRequest req, CancellationToken ct)
        {
            AppUser? appUser = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
            if (appUser == null)
            {
                await SendAsync(string.Format(NotFound, "Account"), Status404NotFound).ConfigureAwait(false);
                return;
            }

            if (appUser.EmailConfirmed)
            {
                await SendAsync(EmailAlreadyVerified, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            string decodedECT = req.Token.Replace(' ', '+');
            IdentityResult result = await manager.ConfirmEmailAsync(appUser, decodedECT).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                await SendAsync(InvalidEmailToken, Status400BadRequest).ConfigureAwait(false);
                return;
            }

            await signInManager.SignInAsync(appUser, false).ConfigureAwait(false);
            UserModel model = await service.GetByName(req.Username).ConfigureAwait(false);

            HttpContext.Response.Cookies.Append("role", model.RoleName);
            HttpContext.Response.Cookies.Append("username", model.UserName);

            JwtSecurityToken jwt = config.GenerateAccessToken(model.Id, model.UserName, model.RoleName);
            string signedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            HttpContext.Response.Cookies.Append("jwt", signedJwt, new() { HttpOnly = true, Secure = true, Expires = jwt.ValidTo });

            (string newRT, DateTime newEnd) = await service.RenewRefreshToken(model).ConfigureAwait(false);
            HttpContext.Response.Cookies.Append("rt", newRT, new() { HttpOnly = true, Secure = true, Expires = newEnd });

            HttpContext.Response.Cookies.Append("role", model.RoleName, new() { Expires = newEnd });
            HttpContext.Response.Cookies.Append("username", model.UserName, new() { Expires = newEnd });

            await SendAsync("Welcome!", Status200OK).ConfigureAwait(false);
        }
    }
}
