using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditById;

public class EditRoleByIdHandler(IRoleQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditRoleByIdCommand>
{
    public async Task Handle(EditRoleByIdCommand req, CancellationToken ct)
    {
        Role role = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {req.Id} does not exist.");

        role.Name = req.Model.Name;
        role.Description = req.Model.Description;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
