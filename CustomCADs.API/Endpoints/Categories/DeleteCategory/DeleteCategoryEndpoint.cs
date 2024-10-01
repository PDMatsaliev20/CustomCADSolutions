using CustomCADs.Application.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.DeleteCategory
{
    using static StatusCodes;

    public class DeleteCategoryEndpoint(ICategoryService service) : Endpoint<DeleteCategoryRequest>
    {
        public override void Configure()
        {
            Delete("{id}");
            Group<CategoriesGroup>();
            Description(s => s
                .WithSummary("Deletes the Category with the provided id.")
                .Produces<EmptyResponse>(Status204NoContent)
                .ProducesProblem(Status500InternalServerError));
        }

        public override async Task HandleAsync(DeleteCategoryRequest req, CancellationToken ct)
        {
            if (await service.ExistsByIdAsync(req.Id).ConfigureAwait(false))
            {
                await service.DeleteAsync(req.Id).ConfigureAwait(false);
            }

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
