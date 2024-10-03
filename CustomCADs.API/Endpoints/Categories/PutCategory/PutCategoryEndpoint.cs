using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.PutCategory
{
    using static StatusCodes;

    public class PutCategoryEndpoint(ICategoryService service) : Endpoint<PutCategoryRequest>
    {
        public override void Configure()
        {
            Put("{id}");
            Group<CategoriesGroup>();
            Description(s => s
                .WithSummary("Edits the name of the Category with the provided id.")
                .Produces<EmptyResponse>(Status204NoContent)
                .ProducesProblem(Status500InternalServerError));
        }

        public override async Task HandleAsync(PutCategoryRequest req, CancellationToken ct)
        {
            CategoryModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            model.Name = req.Name;

            await service.EditAsync(req.Id, model).ConfigureAwait(false);
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
