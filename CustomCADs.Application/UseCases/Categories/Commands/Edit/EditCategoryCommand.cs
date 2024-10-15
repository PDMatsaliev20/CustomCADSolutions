using CustomCADs.Application.Models.Categories;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Edit
{
    public record EditCategoryCommand(int Id, CategoryModel Model) : IRequest { }
}
