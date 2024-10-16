using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetByName;

public class GetRoleByNameHandler(IRoleQueries queries) : IRequestHandler<GetRoleByNameQuery, RoleModel>
{
    public async Task<RoleModel> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        Role role = await queries.GetByNameAsync(request.Name, false).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with name: {request.Name} does not exist.");

        var response = role.Adapt<RoleModel>();
        return response;
    }
}
