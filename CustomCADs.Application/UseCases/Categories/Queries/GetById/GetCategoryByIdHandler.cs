using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.GetById;

public class GetCategoryByIdHandler(ICategoryReads reads) : IRequestHandler<GetCategoryByIdQuery, CategoryModel>
{
    public async Task<CategoryModel> Handle(GetCategoryByIdQuery req, CancellationToken ct)
    {
        Category? category = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(req.Id);

        var response = category.Adapt<CategoryModel>();
        return response;
    }
}
