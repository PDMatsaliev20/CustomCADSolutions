using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteByName;

public class DeleteUserByNameHandler(
    IUserReads reads,
    IWrites<User> writes,
    IUnitOfWork uow) : IRequestHandler<DeleteUserByNameCommand>
{
    public async Task Handle(DeleteUserByNameCommand req, CancellationToken ct)
    {
        User user = await reads.GetByNameAsync(req.Name, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with name: {req.Name} does not exist.");

        writes.Delete(user);
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
