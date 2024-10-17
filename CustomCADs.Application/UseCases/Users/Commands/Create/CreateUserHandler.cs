using CustomCADs.Domain.Shared;
using CustomCADs.Domain.Users;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.Create;

public class CreateUserHandler(ICommands<User> commands, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand req, CancellationToken ct)
    {
        User user = req.Model.Adapt<User>();

        await commands.AddAsync(user, ct).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return user.Id;
    }
}
