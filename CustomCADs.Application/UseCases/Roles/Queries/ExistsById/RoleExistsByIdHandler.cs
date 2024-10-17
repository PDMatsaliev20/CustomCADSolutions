using CustomCADs.Domain.Roles.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsById;

public class RoleExistsByIdHandler(IRoleQueries queries) : IRequestHandler<RoleExistsByIdQuery, bool>
{
    public async Task<bool> Handle(RoleExistsByIdQuery req, CancellationToken ct)
    {
        bool roleExists = await queries.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return roleExists;
    }
}
