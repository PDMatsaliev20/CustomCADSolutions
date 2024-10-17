using CustomCADs.Application.Common.Helpers;
using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Cads;
using Mapster;
using MediatR;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using CustomCADs.Domain.Cads.Enums;
using CustomCADs.Domain.Shared.Enums;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetCadAndAdjacentById;

public class GetCadAndAdjacentByIdHandler(ICadReads reads) : IRequestHandler<GetCadAndAdjacentByIdQuery, (int? PrevId, CadModel Current, int? NextId)>
{
    public Task<(int? PrevId, CadModel Current, int? NextId)> Handle(GetCadAndAdjacentByIdQuery req, CancellationToken ct)
    {
        IQueryable<Cad> queryable = reads.GetAll(asNoTracking: true)
            .Sort(nameof(Sorting.Oldest))
            .Filter(status: nameof(CadStatus.Unchecked));

        List<Cad> cads = [.. queryable];
        Cad cad = queryable.FirstOrDefault(c => c.Id == req.Id)
            ?? throw new CadNotFoundException(req.Id);

        int cadIndex = cads.IndexOf(cad);

        int? prevId = null;
        if (cad.Id != (cads.FirstOrDefault()?.Id ?? 0))
        {
            prevId = cads[cadIndex - 1].Id;
        }

        int? nextId = null;
        if (cad.Id != (cads.LastOrDefault()?.Id ?? 0))
        {
            nextId = cads[cadIndex + 1].Id;
        }

        return Task.FromResult((prevId, cad.Adapt<CadModel>(), nextId));
    }
}
