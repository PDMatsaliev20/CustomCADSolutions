using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Categories.GetCategories
{
    using static StatusCodes;

    public class GetCategoriesEndpoint(ICategoryService service) : EndpointWithoutRequest<IEnumerable<CategoryDto>>
    {
        public override void Configure()
        {
            Get("");
            AllowAnonymous();
            Group<CategoriesGroup>();
            Description(s => s
                .WithSummary("Gets all existing Categories.")
                .Produces<IEnumerable<CategoryDto>>(Status200OK, "application/json")
                .ProducesProblem(Status500InternalServerError));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            IEnumerable<CategoryDto> categories = service.GetAll()
                .Select(c => new CategoryDto(c.Id, c.Name));

            await SendOkAsync(categories).ConfigureAwait(false);
        }
    }
}
