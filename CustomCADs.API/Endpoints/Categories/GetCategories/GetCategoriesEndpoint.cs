using CustomCADs.API.Endpoints.Categories.GetCategoryById;
using CustomCADs.API.Models.Others;
using CustomCADs.Application.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.GetCategories
{
    using static StatusCodes;

    public class GetCategoriesEndpoint(ICategoryService service) : EndpointWithoutRequest<IEnumerable<CategoryDTO>>
    {
        public override void Configure()
        {
            Get("");
            AllowAnonymous();
            Group<CategoriesGroup>();
            Description(s => s.WithSummary("Gets all existing Categories."));
            Options(opt =>
            {
                opt.Produces<IEnumerable<CategoryDTO>>(Status200OK, "application/json");
                opt.ProducesProblem(Status500InternalServerError);
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IEnumerable<CategoryDTO> categories = service.GetAll()
                .Select(c => new CategoryDTO()
                {
                    Id = c.Id,
                    Name = c.Name,
                });

            await SendAsync(categories, Status200OK).ConfigureAwait(false);
        }
    }
}
