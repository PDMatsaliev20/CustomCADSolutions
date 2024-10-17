using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAll;

public class GetAllRolesHandler(IRoleQueries queries) : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleModel>>
{
    public Task<IEnumerable<RoleModel>> Handle(GetAllRolesQuery req, CancellationToken ct)
    {
        IQueryable<Role> queryable = queries.GetAll(asNoTracking: true)
            .Search(req.Name, req.Description)
            .Sort(req.Sorting);

        IEnumerable<Role> roles = [.. queryable];

        return Task.FromResult(roles.Adapt<IEnumerable<RoleModel>>());
    }
}
