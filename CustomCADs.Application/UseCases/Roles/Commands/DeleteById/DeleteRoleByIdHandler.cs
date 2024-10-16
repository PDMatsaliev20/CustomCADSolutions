using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteById;

public class DeleteRoleByIdHandler(
    IRoleQueries queries,
    ICommands<Role> commands,
    IUnitOfWork unitOfWork): IRequestHandler<DeleteRoleByIdCommand>
{
    public async Task Handle(DeleteRoleByIdCommand request, CancellationToken cancellationToken)
    {
        Role role = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {request.Id} does not exist.");

        commands.Delete(role);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
