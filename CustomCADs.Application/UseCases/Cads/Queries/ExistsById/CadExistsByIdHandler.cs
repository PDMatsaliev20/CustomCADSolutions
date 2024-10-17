using CustomCADs.Domain.Cads.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.ExistsById;

public class CadExistsByIdHandler(ICadReads reads) : IRequestHandler<CadExistsByIdQuery, bool>
{
    public async Task<bool> Handle(CadExistsByIdQuery req, CancellationToken ct)
    {
        bool cadExists = await reads.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return cadExists;
    }
}
