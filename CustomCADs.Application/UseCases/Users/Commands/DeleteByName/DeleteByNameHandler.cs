using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteByName;

public class DeleteUserByNameHandler(
    IUserQueries queries,
    ICommands<User> commands,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByNameCommand>
{
    public async Task Handle(DeleteUserByNameCommand req, CancellationToken ct)
    {
        User user = await queries.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with name: {req.Name} does not exist.");

        commands.Delete(user);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
