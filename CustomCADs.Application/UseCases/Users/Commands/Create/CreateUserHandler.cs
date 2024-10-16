using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Users.Commands.Create;

public class CreateUserHandler(ICommands<User> commands, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserCommand, string>
{
    public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        User user = request.Model.Adapt<User>();

        await commands.AddAsync(user).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return user.Id;
    }
}
