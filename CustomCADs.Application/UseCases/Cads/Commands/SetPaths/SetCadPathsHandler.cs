using CustomCADs.Application.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.SetPaths;

public class SetCadPathsHandler(ICadQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<SetCadPathsCommand>
{
    public async Task Handle(SetCadPathsCommand request, CancellationToken cancellationToken)
    {
        Cad cad = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new CadNotFoundException(request.Id);

        cad.Paths = new(request.CadPath, request.ImagePath);

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
