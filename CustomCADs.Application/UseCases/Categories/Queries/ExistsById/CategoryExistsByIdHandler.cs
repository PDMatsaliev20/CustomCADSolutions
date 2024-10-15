using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.ExistsById
{
    public class CategoryExistsByIdHandler(ICategoryQueries queries) : IRequestHandler<CategoryExistsByIdQuery, bool>
    {
        public async Task<bool> Handle(CategoryExistsByIdQuery request, CancellationToken cancellationToken)
        {
            bool categoryExists = await queries.ExistsByIdAsync(request.Id).ConfigureAwait(false);

            return categoryExists;
        }
    }
}
