using CustomCADs.Application.Contracts;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.DeleteRole
{
    using static ApiMessages;
    using static StatusCodes;

    public class DeleteRoleEndpoint(IAppRoleManager manager, IRoleService service) : Endpoint<DeleteRoleRequest>
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
            AppRole? role = await manager.FindByNameAsync(req.Name).ConfigureAwait(false);
            if (role == null || !await service.ExistsByNameAsync(req.Name).ConfigureAwait(false))
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = string.Format(NotFound, "Role"),
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return; 
            }

            await manager.DeleteAsync(role).ConfigureAwait(false);
            await service.DeleteAsync(req.Name).ConfigureAwait(false);
            
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
