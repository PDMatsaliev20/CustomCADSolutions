using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteByName;

public class DeleteRoleByNameHandler(
    IRoleQueries queries,
    ICommands<Role> commands,
    IUnitOfWork unitOfWork): IRequestHandler<DeleteRoleByNameCommand>
{
    public async Task Handle(DeleteRoleByNameCommand req, CancellationToken ct)
    {
        Role role = await queries.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with name: {req.Name} does not exist.");

        commands.Delete(role);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
