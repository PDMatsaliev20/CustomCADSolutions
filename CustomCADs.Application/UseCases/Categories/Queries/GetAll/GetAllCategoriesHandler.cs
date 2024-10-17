using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Queries;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.GetAll;

public class GetAllCategoriesHandler(ICategoryQueries queries) : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryModel>>
{
    public Task<IEnumerable<CategoryModel>> Handle(GetAllCategoriesQuery req, CancellationToken ct)
    {
        IEnumerable<Category> categories = [.. queries.GetAll(asNoTracking: true)];

        var response = categories.Adapt<IEnumerable<CategoryModel>>();
        return Task.FromResult(response);
    }
}
