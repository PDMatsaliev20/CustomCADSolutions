using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Shared;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Commands.Create;

public class CreateRoleHandler(IWrites<Role> writes, IUnitOfWork uow) : IRequestHandler<CreateRoleCommand, string>
{
    public async Task<string> Handle(CreateRoleCommand req, CancellationToken ct)
    {
        Role role = req.Model.Adapt<Role>();

        await writes.AddAsync(role, ct).ConfigureAwait(false);
        await uow.SaveChangesAsync().ConfigureAwait(false);

        return role.Id;
    }
}
