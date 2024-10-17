using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Domain.Roles;
using CustomCADs.Domain.Roles.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetById;

public class GetRoleByIdHandler(IRoleReads reads) : IRequestHandler<GetRoleByIdQuery, RoleModel>
{
    public async Task<RoleModel> Handle(GetRoleByIdQuery req, CancellationToken ct)
    {
        Role role = await reads.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new RoleNotFoundException($"The Role with id: {req.Id} does not exist.");

        var response = role.Adapt<RoleModel>();
        return response;
    }
}
