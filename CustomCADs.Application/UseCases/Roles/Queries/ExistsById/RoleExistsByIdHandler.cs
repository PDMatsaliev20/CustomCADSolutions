using CustomCADs.Domain.Roles.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsById;

public class RoleExistsByIdHandler(IRoleReads reads) : IRequestHandler<RoleExistsByIdQuery, bool>
{
    public async Task<bool> Handle(RoleExistsByIdQuery req, CancellationToken ct)
    {
        bool roleExists = await reads.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return roleExists;
    }
}
