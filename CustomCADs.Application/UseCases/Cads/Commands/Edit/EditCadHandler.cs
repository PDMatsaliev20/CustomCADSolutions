using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Edit;

public class EditCadHandler(ICadQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditCadCommand>
{
    public async Task Handle(EditCadCommand req, CancellationToken ct)
    {
        Cad cad = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CadNotFoundException(req.Id);

        cad.Name = req.Model.Name;
        cad.Description = req.Model.Description;
        cad.Price = req.Model.Price;
        cad.CategoryId = req.Model.CategoryId;
        cad.CamCoordinates = req.Model.CamCoordinates;
        cad.PanCoordinates = req.Model.PanCoordinates;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
