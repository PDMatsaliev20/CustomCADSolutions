using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetById;

public class GetCadByIdHandler(ICadQueries queries) : IRequestHandler<GetCadByIdQuery, CadModel>
{
    public async Task<CadModel> Handle(GetCadByIdQuery req, CancellationToken ct)
    {
        Cad cad = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);
        
        var result = cad.Adapt<CadModel>();
        return result;
    }
}
