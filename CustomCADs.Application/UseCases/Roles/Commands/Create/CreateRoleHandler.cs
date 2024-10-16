using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.Create;

public class CreateRoleHandler(ICommands<Role> commands, IUnitOfWork unitOfWork) : IRequestHandler<CreateRoleCommand, string>
{
    public async Task<string> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        Role role = request.Model.Adapt<Role>();

        await commands.AddAsync(role).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return role.Id;
    }
}
