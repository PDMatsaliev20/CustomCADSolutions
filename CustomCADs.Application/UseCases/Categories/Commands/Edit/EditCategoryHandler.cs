using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Edit;

public class EditCategoryHandler(ICategoryQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditCategoryCommand>
{
    public async Task Handle(EditCategoryCommand req, CancellationToken ct)
    {
        Category category = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(req.Id);

        category.Name = req.Model.Name;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
