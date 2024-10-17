using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using CustomCADs.Domain.Users.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.DeleteById;

public class DeleteUserByIdHandler(
    IUserReads reads,
    IWrites<User> writes,
    IUnitOfWork uow) : IRequestHandler<DeleteUserByIdCommand>
{
    public async Task Handle(DeleteUserByIdCommand req, CancellationToken ct)
    {
        User user = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new UserNotFoundException($"The User with id: {req.Id} does not exist.");

        writes.Delete(user);
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
