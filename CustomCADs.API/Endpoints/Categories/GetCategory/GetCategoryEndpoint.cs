using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.GetCategoryById
{
    using static StatusCodes;

    public class GetCategoryEndpoint(ICategoryService service) : Endpoint<GetCategoryRequest, CategoryDto>
    {
        public override void Configure()
        {
            Get("{id}");
            AllowAnonymous();
            Group<CategoriesGroup>();
            Description(s => s
                .WithSummary("Gets the Category with the provided id.")
                .Produces<GetCategoryRequest>(Status200OK, "application/json")
                .ProducesProblem(Status500InternalServerError));
        }

        public override async Task HandleAsync(GetCategoryRequest req, CancellationToken ct)
        {
            CategoryModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            CategoryDto category = new(model.Id, model.Name);

            await SendOkAsync(category).ConfigureAwait(false);
        }
    }
}
