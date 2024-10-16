using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using CustomCADs.Domain.Enums;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetCadAndAdjacentById;

public class GetCadAndAdjacentByIdHandler(ICadQueries queries) : IRequestHandler<GetCadAndAdjacentByIdQuery, (int? PrevId, CadModel Current, int? NextId)>
{
    public async Task<(int? PrevId, CadModel Current, int? NextId)> Handle(GetCadAndAdjacentByIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Cad> queryable = queries.GetAll(true)
            .Sort(nameof(Sorting.Oldest))
            .Filter(status: nameof(CadStatus.Unchecked));

        List<Cad> cads = [.. queryable];
        Cad cad = queryable.FirstOrDefault(c => c.Id == request.Id)
            ?? throw new CadNotFoundException(request.Id);

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

        return (prevId, cad.Adapt<CadModel>(), nextId);
    }
}
