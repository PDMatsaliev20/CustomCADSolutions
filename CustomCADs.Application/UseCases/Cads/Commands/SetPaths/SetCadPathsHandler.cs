using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.SetPaths;

public class SetCadPathsHandler(ICadReads reads, IUnitOfWork uow) : IRequestHandler<SetCadPathsCommand>
{
    public async Task Handle(SetCadPathsCommand req, CancellationToken ct)
    {
        Cad cad = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);

        cad.Paths = new(req.CadPath, req.ImagePath);

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
