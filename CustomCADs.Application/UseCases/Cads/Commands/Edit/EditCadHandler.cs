using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Cads.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Edit;

public class EditCadHandler(ICadReads reads, IUnitOfWork uow) : IRequestHandler<EditCadCommand>
{
    public async Task Handle(EditCadCommand req, CancellationToken ct)
    {
        Cad cad = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);

        cad.Name = req.Model.Name;
        cad.Description = req.Model.Description;
        cad.Price = req.Model.Price;
        cad.CategoryId = req.Model.CategoryId;
        cad.CamCoordinates = req.Model.CamCoordinates;
        cad.PanCoordinates = req.Model.PanCoordinates;

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
