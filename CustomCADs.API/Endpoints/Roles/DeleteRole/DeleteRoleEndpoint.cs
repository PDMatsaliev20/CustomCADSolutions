using CustomCADs.Application.Contracts;
using CustomCADs.Application.UseCases.Roles.Commands.DeleteByName;
using CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Roles.DeleteRole;

using static ApiMessages;
using static StatusCodes;

public class DeleteRoleEndpoint(IMediator mediator, IAppRoleManager manager) : Endpoint<DeleteRoleRequest>
{
    public override void Configure()
    {
        Delete("{name}");
        Group<RolesGroup>();
        Description(d => d
            .WithSummary("Deletes the Role with the specified name.")
            .Produces<EmptyResponse>(Status204NoContent)
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(DeleteRoleRequest req, CancellationToken ct)
    {
        RoleExistsByNameQuery query = new(req.Name);
        bool roleExists = await mediator.Send(query).ConfigureAwait(false);

        AppRole? role = await manager.FindByNameAsync(req.Name).ConfigureAwait(false);
        if (role == null || !roleExists)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(NotFound, "Role"),
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return; 
        }

        DeleteRoleByNameCommand command = new(req.Name);
        await mediator.Send(command).ConfigureAwait(false);
        await manager.DeleteAsync(role).ConfigureAwait(false);
        
        await SendNoContentAsync().ConfigureAwait(false);
    }
}
