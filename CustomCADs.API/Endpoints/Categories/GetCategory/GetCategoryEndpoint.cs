using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.UseCases.Categories.Queries.GetById;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Categories.GetCategoryById;

using static StatusCodes;

public class GetCategoryEndpoint(IMediator mediator) : Endpoint<GetCategoryRequest, CategoryDto>
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
        GetCategoryByIdQuery query = new(req.Id);
        CategoryModel model = await mediator.Send(query).ConfigureAwait(false);
        
        CategoryDto response = new(model.Id, model.Name);
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
