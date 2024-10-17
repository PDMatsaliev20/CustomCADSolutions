using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Edit;

public class EditCategoryHandler(ICategoryQueries queries, IUnitOfWork unitOfWork) : IRequestHandler<EditCategoryCommand>
{
    public async Task Handle(EditCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(request.Id);

        category.Name = request.Model.Name;

        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
    }
}
