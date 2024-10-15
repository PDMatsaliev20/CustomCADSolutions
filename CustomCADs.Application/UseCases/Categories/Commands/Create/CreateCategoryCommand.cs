using CustomCADs.Application.Models.Categories;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Create
{
    public record CreateCategoryCommand(CategoryModel Model) : IRequest<int> { }
}
