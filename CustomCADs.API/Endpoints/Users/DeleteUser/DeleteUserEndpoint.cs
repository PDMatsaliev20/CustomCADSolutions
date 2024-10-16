using CustomCADs.Application.UseCases.Users.Commands.DeleteByName;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Users.DeleteUser;

using static StatusCodes;

public class DeleteUserEndpoint(IMediator mediator, IAppUserManager manager) : Endpoint<DeleteUserRequest>
{
    public override void Configure()
    {
        Delete("{username}");
        Group<UsersGroup>();
        Description(d => d
            .WithSummary("Deletes the User with the specified username.")
            .Produces<EmptyResponse>(Status204NoContent)
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(DeleteUserRequest req, CancellationToken ct)
    {
        AppUser? user = await manager.FindByNameAsync(req.Username).ConfigureAwait(false);
        if (user == null)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(ApiMessages.NotFound, "User"),
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return; 
        }

        DeleteUserByNameCommand command = new(req.Username);
        await mediator.Send(command).ConfigureAwait(false);
        await manager.DeleteAsync(user).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
