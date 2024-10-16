using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;

public class RoleExistsByNameHandler(IRoleQueries queries) : IRequestHandler<RoleExistsByNameQuery, bool>
{
    public async Task<bool> Handle(RoleExistsByNameQuery request, CancellationToken cancellationToken)
    {
        bool roleExists = await queries.ExistsByNameAsync(request.Name).ConfigureAwait(false);

        return roleExists;
    }
}
