using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteById;

public class DeleteUserByIdHandler(
    IUserQueries queries,
    ICommands<User> commands,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByIdCommand>
{
    public async Task Handle(DeleteUserByIdCommand req, CancellationToken ct)
    {
        User user = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {req.Id} does not exist.");

        commands.Delete(user);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
