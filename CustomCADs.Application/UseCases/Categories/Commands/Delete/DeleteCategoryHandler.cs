using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Queries;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Delete;

public class DeleteCategoryHandler(ICategoryQueries queries, ICommands<Category> commands, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand req, CancellationToken ct)
    {
        Category category = await queries.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(req.Id);

        commands.Delete(category);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
