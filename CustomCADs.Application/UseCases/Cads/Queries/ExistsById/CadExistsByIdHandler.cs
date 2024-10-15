using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.ExistsById
{
    public class CadExistsByIdHandler(ICadQueries queries) : IRequestHandler<CadExistsByIdQuery, bool>
    {
        public async Task<bool> Handle(CadExistsByIdQuery request, CancellationToken cancellationToken)
        {
            bool cadExists = await queries.ExistsByIdAsync(request.Id).ConfigureAwait(false);

            return cadExists;
        }
    }
}
