using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAllNames;

public class GetAllRolesHandler(IRoleQueries queries) : IRequestHandler<GetAllRoleNamesQuery, IEnumerable<string>>
{
    public Task<IEnumerable<string>> Handle(GetAllRoleNamesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<string> queryable = queries.GetAll(true)
            .Select(r => r.Name);

        IEnumerable<string> roleNames = [.. queryable ];

        return Task.FromResult(roleNames);
    }
}
