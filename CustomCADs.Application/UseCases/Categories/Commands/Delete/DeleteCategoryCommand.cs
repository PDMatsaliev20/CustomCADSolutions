using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Delete
{
    public record DeleteCategoryCommand(int Id) : IRequest { }
}
