using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditByName;

public class EditRoleByNameHandler(IRoleQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditRoleByNameCommand>
{
    public async Task Handle(EditRoleByNameCommand request, CancellationToken cancellationToken)
    {
        Role role = await queries.GetByNameAsync(request.Name).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with name: {request.Name} does not exist.");

        role.Name = request.Model.Name;
        role.Description = request.Model.Description;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}