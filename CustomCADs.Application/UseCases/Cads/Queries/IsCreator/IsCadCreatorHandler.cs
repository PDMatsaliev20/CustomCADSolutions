using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.IsCreator;

public class IsCadCreatorHandler(ICadQueries queries) : IRequestHandler<IsCadCreatorQuery, bool>
{
    public async Task<bool> Handle(IsCadCreatorQuery req, CancellationToken ct)
    {
        Cad cad = await queries.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);

        var result = cad.Creator.UserName == req.Username;
        return result;
    }
}
