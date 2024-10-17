using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteById;

public class DeleteRoleByIdHandler(
    IRoleReads reads,
    IWrites<Role> writes,
    IUnitOfWork uow): IRequestHandler<DeleteRoleByIdCommand>
{
    public async Task Handle(DeleteRoleByIdCommand req, CancellationToken ct)
    {
        Role role = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {req.Id} does not exist.");

        writes.Delete(role);
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
