using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.ExistsById;

public class CadExistsByIdHandler(ICadQueries queries) : IRequestHandler<CadExistsByIdQuery, bool>
{
    public async Task<bool> Handle(CadExistsByIdQuery req, CancellationToken ct)
    {
        bool cadExists = await queries.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return cadExists;
    }
}
