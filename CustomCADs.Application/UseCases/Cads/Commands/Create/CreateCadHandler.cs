using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Create;

public class CreateCadHandler(
    ICategoryQueries queries,
    ICommands<Cad> commands, 
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCadCommand, int>
{
    public async Task<int> Handle(CreateCadCommand request, CancellationToken cancellationToken)
    {
        bool categoryExists = await queries.ExistsByIdAsync(request.Model.CategoryId).ConfigureAwait(false);
        if (!categoryExists)
        {
            throw new CategoryNotFoundException(request.Model.CategoryId);
        }

        Cad cad = new()
        {
            Name = request.Model.Name,
            Description = request.Model.Description,
            CategoryId = request.Model.CategoryId,
            Price = request.Model.Price,
            CreatorId = request.Model.CreatorId,
            Status = request.Model.Status,
            CreationDate = DateTime.Now,
        };
        await commands.AddAsync(cad).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        return cad.Id;
    }
}
