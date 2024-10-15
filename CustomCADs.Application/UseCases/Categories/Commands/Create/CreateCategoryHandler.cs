using CustomCADs.Domain.Contracts;
using CustomCADs.Domain.Entities;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Create;

public class CreateCategoryHandler(ICommands<Category> commands, IUnitOfWork unitOfWork) : IRequestHandler<CreateCategoryCommand, int>
{
    public async Task<int> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = request.Model.Adapt<Category>();
        await commands.AddAsync(category).ConfigureAwait(false);
        await unitOfWork.SaveChangesAsync().ConfigureAwait(false);
        
        var response = category.Id;
        return response;
    }
}
