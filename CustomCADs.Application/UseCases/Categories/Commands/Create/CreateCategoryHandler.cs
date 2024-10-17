using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Shared;
using Mapster;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Create;

public class CreateCategoryHandler(IWrites<Category> writes, IUnitOfWork uow) : IRequestHandler<CreateCategoryCommand, int>
{
    public async Task<int> Handle(CreateCategoryCommand req, CancellationToken ct)
    {
        Category category = req.Model.Adapt<Category>();
        await writes.AddAsync(category, ct).ConfigureAwait(false);
        await uow.SaveChangesAsync().ConfigureAwait(false);
        
        var response = category.Id;
        return response;
    }
}
