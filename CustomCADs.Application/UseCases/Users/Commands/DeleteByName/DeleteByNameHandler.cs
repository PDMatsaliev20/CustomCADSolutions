using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteByName;

public class DeleteUserByNameHandler(
    IUserQueries queries,
    ICommands<User> commands,
    IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserByNameCommand>
{
    public async Task Handle(DeleteUserByNameCommand request, CancellationToken cancellationToken)
    {
        User user = await queries.GetByNameAsync(request.Name).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with name: {request.Name} does not exist.");

        commands.Delete(user);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
