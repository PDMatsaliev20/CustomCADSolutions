using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Edit;

public class EditCadHandler(ICadQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditCadCommand>
{
    public async Task Handle(EditCadCommand request, CancellationToken cancellationToken)
    {
        Cad cad = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new CadNotFoundException(request.Id);

        cad.Name = request.Model.Name;
        cad.Description = request.Model.Description;
        cad.Price = request.Model.Price;
        cad.CategoryId = request.Model.CategoryId;
        cad.CamCoordinates = request.Model.CamCoordinates;
        cad.PanCoordinates = request.Model.PanCoordinates;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
