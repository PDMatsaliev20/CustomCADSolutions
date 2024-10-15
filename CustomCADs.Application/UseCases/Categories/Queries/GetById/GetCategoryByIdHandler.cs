using CustomCADs.Application.Exceptions;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Contracts.Queries;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.GetById;

public class GetCategoryByIdHandler(ICategoryQueries queries) : IRequestHandler<GetCategoryByIdQuery, CategoryModel>
{
    public async Task<CategoryModel> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        Category? category = await queries.GetByIdAsync(request.Id).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(request.Id);

        var response = category.Adapt<CategoryModel>();
        return response;
    }
}
