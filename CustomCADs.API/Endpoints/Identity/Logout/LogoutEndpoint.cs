using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Auth;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Identity.Logout
{
    using static StatusCodes;

    public class LogoutEndpoint(IUserService service, SignInManager<AppUser> signInManager) : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Post("Logout");
            Group<IdentityGroup>();
            Description(d => d
                .WithSummary("Logs out of the current account.")
                .Produces<EmptyResponse>(Status200OK)
                .ProducesProblem(Status400BadRequest));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            UserModel model = await service.GetByIdAsync(User.GetId()).ConfigureAwait(false);
            await signInManager.SignOutAsync().ConfigureAwait(false);

            model.RefreshToken = null;
            model.RefreshTokenEndDate = null;
            await service.EditAsync(model.UserName, model);

            HttpContext.Response.Cookies.Delete("jwt");
            HttpContext.Response.Cookies.Delete("rt");
            HttpContext.Response.Cookies.Delete("username");
            HttpContext.Response.Cookies.Delete("role");

            await SendOkAsync("Bye-bye.").ConfigureAwait(false);
        }
    }
}
