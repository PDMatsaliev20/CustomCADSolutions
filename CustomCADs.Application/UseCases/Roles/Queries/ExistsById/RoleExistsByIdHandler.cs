using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsById;

public class RoleExistsByIdHandler(IRoleQueries queries) : IRequestHandler<RoleExistsByIdQuery, bool>
{
    public async Task<bool> Handle(RoleExistsByIdQuery request, CancellationToken cancellationToken)
    {
        bool roleExists = await queries.ExistsByIdAsync(request.Id).ConfigureAwait(false);

        return roleExists;
    }
}
