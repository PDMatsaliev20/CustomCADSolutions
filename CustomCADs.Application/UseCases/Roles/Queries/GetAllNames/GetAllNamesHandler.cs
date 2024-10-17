using CustomCADs.Domain.Roles.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Roles.Queries.GetAllNames;

public class GetAllNamesHandler(IRoleReads reads) : IRequestHandler<GetAllRoleNamesQuery, IEnumerable<string>>
{
    public Task<IEnumerable<string>> Handle(GetAllRoleNamesQuery req, CancellationToken ct)
    {
        IQueryable<string> queryable = reads.GetAll(asNoTracking: true)
            .Select(r => r.Name);

        IEnumerable<string> roleNames = [.. queryable ];

        return Task.FromResult(roleNames);
    }
}
