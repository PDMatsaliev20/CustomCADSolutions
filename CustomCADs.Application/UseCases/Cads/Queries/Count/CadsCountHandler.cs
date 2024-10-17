using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Cads.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.Count;

public class CadsCountHandler(ICadReads reads) : IRequestHandler<CadsCountQuery, int>
{
    public Task<int> Handle(CadsCountQuery req, CancellationToken ct)
    {
        int count = reads.Count(cad => req.Predicate(cad.Adapt<CadModel>()));

        return Task.FromResult(count);
    }
}
