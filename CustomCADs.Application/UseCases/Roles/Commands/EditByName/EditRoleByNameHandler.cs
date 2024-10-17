using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditByName;

public class EditRoleByNameHandler(IRoleQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditRoleByNameCommand>
{
    public async Task Handle(EditRoleByNameCommand req, CancellationToken ct)
    {
        Role role = await queries.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with name: {req.Name} does not exist.");

        role.Name = req.Model.Name;
        role.Description = req.Model.Description;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}