using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditById;

public class EditRoleByIdHandler(IRoleReads reads, IUnitOfWork uow) : IRequestHandler<EditRoleByIdCommand>
{
    public async Task Handle(EditRoleByIdCommand req, CancellationToken ct)
    {
        Role role = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {req.Id} does not exist.");

        role.Name = req.Model.Name;
        role.Description = req.Model.Description;

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
