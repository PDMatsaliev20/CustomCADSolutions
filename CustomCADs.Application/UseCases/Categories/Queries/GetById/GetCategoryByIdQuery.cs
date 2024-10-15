using CustomCADs.Application.Models.Categories;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Queries.GetById
{
    public record GetCategoryByIdQuery(int Id) : IRequest<CategoryModel>;
}
