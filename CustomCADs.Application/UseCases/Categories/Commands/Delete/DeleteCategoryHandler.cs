using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Delete;

public class DeleteCategoryHandler(ICategoryQueries queries, ICommands<Category> commands, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(request.Id);

        commands.Delete(category);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
