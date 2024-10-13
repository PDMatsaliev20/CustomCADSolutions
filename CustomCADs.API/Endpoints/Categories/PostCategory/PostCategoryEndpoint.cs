using CustomCADs.API.Endpoints.Categories.GetCategoryById;
using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.PostCategory
{
    using static StatusCodes;

    public class PostCategoryEndpoint(ICategoryService service) : Endpoint<PostCategoryRequest, CategoryDto>
    {
        public override void Configure()
        {
            Post("");
            Group<CategoriesGroup>();
            Description(s => s
                .WithSummary("Creates a new Category with the provided name.")
                .Produces<GetCategoryRequest>(Status200OK, "application/json")
                .ProducesProblem(Status500InternalServerError));
        }

        public override async Task HandleAsync(PostCategoryRequest req, CancellationToken ct)
        {
            int id = await service.CreateAsync(new() { Name = req.Name }).ConfigureAwait(false);
            CategoryDto category = new(id, req.Name);
            await SendCreatedAtAsync<GetCategoryEndpoint>(new { id }, category).ConfigureAwait(false);
        }
    }
}
