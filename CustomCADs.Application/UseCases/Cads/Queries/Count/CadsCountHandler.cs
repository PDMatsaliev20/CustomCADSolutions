using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.Count;

public class CadsCountHandler(ICadQueries queries) : IRequestHandler<CadsCountQuery, int>
{
    public async Task<int> Handle(CadsCountQuery request, CancellationToken cancellationToken)
    {
        int count = await queries.CountAsync(
            cad => request.Predicate(cad.Adapt<CadModel>()),
            asNoTracking: true
        ).ConfigureAwait(false);

        return count;
    }
}
