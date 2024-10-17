using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.IsCreator;

public class IsCadCreatorHandler(ICadReads reads) : IRequestHandler<IsCadCreatorQuery, bool>
{
    public async Task<bool> Handle(IsCadCreatorQuery req, CancellationToken ct)
    {
        Cad cad = await reads.GetByIdAsync(req.Id, asNoTracking: true, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);

        var result = cad.Creator.UserName == req.Username;
        return result;
    }
}
