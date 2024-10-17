using CustomCADs.Domain.Categories.Reads;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.ExistsById;

public class CategoryExistsByIdHandler(ICategoryReads reads) : IRequestHandler<CategoryExistsByIdQuery, bool>
{
    public async Task<bool> Handle(CategoryExistsByIdQuery req, CancellationToken ct)
    {
        bool categoryExists = await reads.ExistsByIdAsync(req.Id, ct: ct).ConfigureAwait(false);

        return categoryExists;
    }
}
