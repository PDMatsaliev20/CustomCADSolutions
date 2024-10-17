using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.SetPaths;

public class SetCadPathsHandler(ICadQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetCadPathsCommand>
{
    public async Task Handle(SetCadPathsCommand req, CancellationToken ct)
    {
        Cad cad = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);

        cad.Paths = new(req.CadPath, req.ImagePath);

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
