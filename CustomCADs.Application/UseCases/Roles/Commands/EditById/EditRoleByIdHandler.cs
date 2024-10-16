using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.EditById;

public class EditRoleByIdHandler(IRoleQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditRoleByIdCommand>
{
    public async Task Handle(EditRoleByIdCommand request, CancellationToken cancellationToken)
    {
        Role role = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {request.Id} does not exist.");

        role.Name = request.Model.Name;
        role.Description = request.Model.Description;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
