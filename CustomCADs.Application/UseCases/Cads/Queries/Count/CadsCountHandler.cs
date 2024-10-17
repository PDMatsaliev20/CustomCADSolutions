using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.Count;

public class CadsCountHandler(ICadQueries queries) : IRequestHandler<CadsCountQuery, int>
{
    public Task<int> Handle(CadsCountQuery req, CancellationToken ct)
    {
        int count = queries.Count(cad => req.Predicate(cad.Adapt<CadModel>()));

        return Task.FromResult(count);
    }
}
