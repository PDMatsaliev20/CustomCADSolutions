using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Edit;

public class EditCategoryHandler(ICategoryReads reads, IUnitOfWork uow) : IRequestHandler<EditCategoryCommand>
{
    public async Task Handle(EditCategoryCommand req, CancellationToken ct)
    {
        Category category = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(req.Id);

        category.Name = req.Model.Name;

        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
