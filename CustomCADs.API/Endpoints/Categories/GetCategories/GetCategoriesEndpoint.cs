using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.UseCases.Categories.Queries.GetAll;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Categories.GetCategories;

using static StatusCodes;

public class GetCategoriesEndpoint(IMediator mediator) : EndpointWithoutRequest<IEnumerable<CategoryDto>>
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
        GetAllCategoriesQuery query = new();
        IEnumerable<CategoryModel> categories = await mediator.Send(query, ct).ConfigureAwait(false);

        var response = categories.Select(c => new CategoryDto(c.Id, c.Name));
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
