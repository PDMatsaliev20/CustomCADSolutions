using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Queries.IsCreator
{
    public class IsCadCreatorHandler(ICadQueries queries) : IRequestHandler<IsCadCreatorQuery, bool>
    {
        public async Task<bool> Handle(IsCadCreatorQuery request, CancellationToken cancellationToken)
        {
            Cad cad = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
                ?? throw new CadNotFoundException(request.Id);

            var result = cad.Creator.UserName == request.Username;
            return result;
        }
    }
}
