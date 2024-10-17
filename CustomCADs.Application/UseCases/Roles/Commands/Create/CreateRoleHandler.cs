using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Shared;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.Create;

public class CreateRoleHandler(ICommands<Role> commands, IUnitOfWork unitOfWork) : IRequestHandler<CreateRoleCommand, string>
{
    public async Task<string> Handle(CreateRoleCommand req, CancellationToken ct)
    {
        Role role = req.Model.Adapt<Role>();

        await commands.AddAsync(role, ct).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);

        return role.Id;
    }
}
