using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetById;

public class GetRoleByIdHandler(IRoleQueries queries) : IRequestHandler<GetRoleByIdQuery, RoleModel>
{
    public async Task<RoleModel> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        Role role = await queries.GetByIdAsync(request.Id, asNoTracking: true).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {request.Id} does not exist.");

        var response = role.Adapt<RoleModel>();
        return response;
    }
}
