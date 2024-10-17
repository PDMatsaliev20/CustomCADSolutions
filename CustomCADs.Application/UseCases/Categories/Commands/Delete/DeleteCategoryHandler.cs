using CustomCADs.Application.Common.Exceptions;
using CustomCADs.Domain.Categories;
using CustomCADs.Domain.Categories.Reads;
using CustomCADs.Domain.Shared;
using MediatR;

namespace CustomCADs.Application.UseCases.Categories.Commands.Delete;

public class DeleteCategoryHandler(
    ICategoryReads reads, 
    IWrites<Category> writes, 
    IUnitOfWork uow) : IRequestHandler<DeleteCategoryCommand>
{
    public async Task Handle(DeleteCategoryCommand req, CancellationToken ct)
    {
        Category category = await reads.GetByIdAsync(req.Id, ct: ct).ConfigureAwait(false)
            ?? throw new CategoryNotFoundException(req.Id);

        writes.Delete(category);
        await uow.SaveChangesAsync().ConfigureAwait(false);
    }
}
