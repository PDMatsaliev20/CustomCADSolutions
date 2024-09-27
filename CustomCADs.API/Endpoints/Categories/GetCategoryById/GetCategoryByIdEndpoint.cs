using CustomCADs.API.Models.Others;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Categories;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.GetCategoryById
{
    using static StatusCodes;

    public class GetCategoryByIdEndpoint(ICategoryService service) : Endpoint<GetCategoryByIdRequest, CategoryDTO>
    {
        public override void Configure()
        {
            Get("{id}");
            AllowAnonymous();
            Group<CategoriesGroup>();
            Description(s => s.WithSummary("Gets the Category with the provided id."));
            Options(opt =>
            {
                opt.Produces<GetCategoryByIdRequest>(Status200OK, "application/json");
                opt.ProducesProblem(Status500InternalServerError);
            });
        }

        public override async Task HandleAsync(GetCategoryByIdRequest req, CancellationToken ct)
        {
            CategoryModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            CategoryDTO category = new()
            {
                Id = model.Id,
                Name = model.Name,
            };

            await SendAsync(category, Status200OK).ConfigureAwait(false);
        }
    }
}
