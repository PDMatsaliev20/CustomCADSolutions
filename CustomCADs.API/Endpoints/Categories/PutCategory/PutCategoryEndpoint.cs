using CustomCADs.Application.Models.Categories;
using CustomCADs.Application.UseCases.Categories.Commands.Edit;
using CustomCADs.Application.UseCases.Categories.Queries.GetById;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Categories.PutCategory;

using static StatusCodes;

public class PutCategoryEndpoint(IMediator mediator) : Endpoint<PutCategoryRequest>
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
        GetCategoryByIdQuery query = new(req.Id);
        CategoryModel model = await mediator.Send(query, ct).ConfigureAwait(false);

        model.Name = req.Name;

        EditCategoryCommand command = new(req.Id, model);
        await mediator.Send(command, ct).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
