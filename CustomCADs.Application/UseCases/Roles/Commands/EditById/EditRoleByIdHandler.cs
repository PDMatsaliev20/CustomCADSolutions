using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
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
