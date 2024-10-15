using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Categories.GetCategoryById;
using CustomCADs.Application.UseCases.Categories.Commands.Create;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Categories.PostCategory;

using static StatusCodes;

public class PostCategoryEndpoint(IMediator mediator) : Endpoint<PostCategoryRequest, CategoryDto>
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
        CreateCategoryCommand command = new(new() { Name = req.Name });
        int id = await mediator.Send(command).ConfigureAwait(false);

        CategoryDto response = new(id, req.Name);
        await SendCreatedAtAsync<GetCategoryEndpoint>(new { id }, response).ConfigureAwait(false);
    }
}
