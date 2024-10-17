using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;

public class RoleExistsByNameHandler(IRoleQueries queries) : IRequestHandler<RoleExistsByNameQuery, bool>
{
    public async Task<bool> Handle(RoleExistsByNameQuery req, CancellationToken ct)
    {
        bool roleExists = await queries.ExistsByNameAsync(req.Name, ct: ct).ConfigureAwait(false);

        return roleExists;
    }
}
