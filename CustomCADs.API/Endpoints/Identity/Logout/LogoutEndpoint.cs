using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Users.Commands.EditByName;
using CustomCADs.Application.UseCases.Users.Queries.GetById;
using CustomCADs.Auth;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Identity.Logout;

using static StatusCodes;

public class LogoutEndpoint(IMediator mediator, SignInManager<AppUser> signInManager) : EndpointWithoutRequest
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
        GetUserByIdQuery query = new(User.GetId());
        UserModel model = await mediator.Send(query, ct).ConfigureAwait(false);
        await signInManager.SignOutAsync().ConfigureAwait(false);

        model.RefreshToken = null;
        model.RefreshTokenEndDate = null;

        EditUserByNameCommand command = new(model.UserName, model);
        await mediator.Send(command, ct).ConfigureAwait(false);

        HttpContext.Response.Cookies.Delete("jwt");
        HttpContext.Response.Cookies.Delete("rt");
        HttpContext.Response.Cookies.Delete("username");
        HttpContext.Response.Cookies.Delete("role");

        await SendOkAsync("Bye-bye.").ConfigureAwait(false);
    }
}
