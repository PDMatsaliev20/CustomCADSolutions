using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.GetById;

public class GetCadByIdHandler(ICadQueries queries) : IRequestHandler<GetCadByIdQuery, CadModel>
{
    public async Task<CadModel> Handle(GetCadByIdQuery request, CancellationToken cancellationToken)
    {
        Cad cad = await queries.GetByIdAsync(request.Id, true).ConfigureAwait(false)
            ?? throw new CadNotFoundException(request.Id);
        ;
        ;
        var result = cad.Adapt<CadModel>();
        return result;
    }
}
