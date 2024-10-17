using CustomCADs.Domain.Roles.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;

public class RoleExistsByNameHandler(IRoleReads reads) : IRequestHandler<RoleExistsByNameQuery, bool>
{
    public async Task<bool> Handle(RoleExistsByNameQuery req, CancellationToken ct)
    {
        bool roleExists = await reads.ExistsByNameAsync(req.Name, ct: ct).ConfigureAwait(false);

        return roleExists;
    }
}
