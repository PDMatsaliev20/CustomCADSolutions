using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Cads;
using CustomCADs.Domain.Categories.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Cads.Commands.Create;

public class CreateCadHandler(
    ICategoryReads categoryReads,
    IWrites<Cad> cadWrites, 
    IUnitOfWork uow) : IRequestHandler<CreateCadCommand, int>
{
    public async Task<int> Handle(CreateCadCommand req, CancellationToken ct)
    {
        bool categoryExists = await categoryReads.ExistsByIdAsync(req.Model.CategoryId, ct: ct).ConfigureAwait(false);
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
        await cadWrites.AddAsync(cad, ct).ConfigureAwait(false);

        await uow.SaveChangesAsync().ConfigureAwait(false);
        return cad.Id;
    }
}
