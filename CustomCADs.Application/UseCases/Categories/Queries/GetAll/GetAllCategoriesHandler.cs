using CustomCADs.Application.Models.Categories;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Reads;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.GetAll;

public class GetAllCategoriesHandler(ICategoryReads reads) : IRequestHandler<GetAllCategoriesQuery, IEnumerable<CategoryModel>>
{
    public Task<IEnumerable<CategoryModel>> Handle(GetAllCategoriesQuery req, CancellationToken ct)
    {
        IEnumerable<Category> categories = [.. reads.GetAll(asNoTracking: true)];

        var response = categories.Adapt<IEnumerable<CategoryModel>>();
        return Task.FromResult(response);
    }
}
