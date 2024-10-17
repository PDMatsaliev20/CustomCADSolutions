using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAll;

public class GetAllRolesHandler(IRoleQueries queries) : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleModel>>
{
    public Task<IEnumerable<RoleModel>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Role> queryable = queries.GetAll(true)
            .Search(request.Name, request.Description)
            .Sort(request.Sorting);

        IEnumerable<Role> roles = [.. queryable];

        return Task.FromResult(roles.Adapt<IEnumerable<RoleModel>>());
    }
}
