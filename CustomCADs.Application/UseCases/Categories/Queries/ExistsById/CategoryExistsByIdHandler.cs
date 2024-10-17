using CustomCADs.Domain.Contracts.Queries;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.ExistsById;

public class CategoryExistsByIdHandler(ICategoryQueries queries) : IRequestHandler<CategoryExistsByIdQuery, bool>
{
    public async Task<bool> Handle(CategoryExistsByIdQuery req, CancellationToken ct)
    {
        bool categoryExists = await queries.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return categoryExists;
    }
}
