using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.Create;

public class CreateUserHandler(IWrites<User> writes, IUnitOfWork uow) : IRequestHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand req, CancellationToken ct)
    {
        User user = req.Model.Adapt<User>();

        await writes.AddAsync(user, ct).ConfigureAwait(false);
        await uow.SaveChangesAsync().ConfigureAwait(false);

        return user.Id;
    }
}
