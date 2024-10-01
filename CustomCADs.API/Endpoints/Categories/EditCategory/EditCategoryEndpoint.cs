using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;

namespace CustomCADs.API.Endpoints.Categories.EditCategory
{
    using static StatusCodes;

    public class EditCategoryEndpoint(ICategoryService service) : Endpoint<EditCategoryRequest>
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

        public override async Task HandleAsync(EditCategoryRequest req, CancellationToken ct)
        {
            CategoryModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            model.Name = req.Name;

            await service.EditAsync(req.Id, model).ConfigureAwait(false);
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
