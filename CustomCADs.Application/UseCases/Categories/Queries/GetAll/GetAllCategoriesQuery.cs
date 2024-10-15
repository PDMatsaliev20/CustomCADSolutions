using CustomCADs.Application.Models.Categories;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.GetAll
{
    public record GetAllCategoriesQuery : IRequest<IEnumerable<CategoryModel>> { }
}
