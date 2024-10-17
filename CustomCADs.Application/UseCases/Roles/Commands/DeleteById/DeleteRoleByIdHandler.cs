using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteById;

public class DeleteRoleByIdHandler(
    IRoleQueries queries,
    ICommands<Role> commands,
    IUnitOfWork unitOfWork): IRequestHandler<DeleteRoleByIdCommand>
{
    public async Task Handle(DeleteRoleByIdCommand req, CancellationToken ct)
    {
        Role role = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {req.Id} does not exist.");

        commands.Delete(role);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
