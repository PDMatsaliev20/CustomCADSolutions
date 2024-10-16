using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.DeleteByName;

public class DeleteRoleByNameHandler(
    IRoleQueries queries,
    ICommands<Role> commands,
    IUnitOfWork unitOfWork): IRequestHandler<DeleteRoleByNameCommand>
{
    public async Task Handle(DeleteRoleByNameCommand request, CancellationToken cancellationToken)
    {
        Role role = await queries.GetByNameAsync(request.Name).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with name: {request.Name} does not exist.");

        commands.Delete(role);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
