using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteByName;

public class DeleteRoleByNameHandler(
    IRoleReads reads,
    IWrites<Role> writes,
    IUnitOfWork uow): IRequestHandler<DeleteRoleByNameCommand>
{
    public async Task Handle(DeleteRoleByNameCommand req, CancellationToken ct)
    {
        Role role = await reads.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with name: {req.Name} does not exist.");

        writes.Delete(role);
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
