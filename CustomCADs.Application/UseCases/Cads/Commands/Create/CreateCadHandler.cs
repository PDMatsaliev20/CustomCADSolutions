using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Categories.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Create;

public class CreateCadHandler(
    ICategoryQueries queries,
    ICommands<Cad> commands, 
    IUnitOfWork unitOfWork) : IRequestHandler<CreateCadCommand, int>
{
    public async Task<int> Handle(CreateCadCommand req, CancellationToken ct)
    {
        bool categoryExists = await queries.ExistsByIdAsync(req.Model.CategoryId, ct: ct).ConfigureAwait(false);
        if (!categoryExists)
        {
            throw new CategoryNotFoundException(req.Model.CategoryId);
        }

        Cad cad = new()
        {
            Name = req.Model.Name,
            Description = req.Model.Description,
            CategoryId = req.Model.CategoryId,
            Price = req.Model.Price,
            CreatorId = req.Model.CreatorId,
            Status = req.Model.Status,
            CreationDate = DateTime.Now,
        };
        await commands.AddAsync(cad, ct).ConfigureAwait(false);

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        return cad.Id;
    }
}
