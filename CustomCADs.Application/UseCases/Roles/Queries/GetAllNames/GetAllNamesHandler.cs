using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAllNames;

public class GetAllNamesHandler(IRoleQueries queries) : IRequestHandler<GetAllRoleNamesQuery, IEnumerable<string>>
{
    public Task<IEnumerable<string>> Handle(GetAllRoleNamesQuery req, CancellationToken ct)
    {
        IQueryable<string> queryable = queries.GetAll(asNoTracking: true)
            .Select(r => r.Name);

        IEnumerable<string> roleNames = [.. queryable ];

        return Task.FromResult(roleNames);
    }
}
