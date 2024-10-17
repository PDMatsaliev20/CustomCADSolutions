using CustomCADs.Application.UseCases.Categories.Commands.Delete;
using CustomCADs.Application.UseCases.Categories.Queries.ExistsById;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Categories.DeleteCategory;

using static ApiMessages;
using static StatusCodes;

public class DeleteCategoryEndpoint(IMediator mediator) : Endpoint<DeleteCategoryRequest>
{
    public override void Configure()
    {
        Delete("{id}");
        Group<CategoriesGroup>();
        Description(s => s
            .WithSummary("Deletes the Category with the provided id.")
            .Produces<EmptyResponse>(Status204NoContent)
            .ProducesProblem(Status500InternalServerError));
    }

    public override async Task HandleAsync(DeleteCategoryRequest req, CancellationToken ct)
    {
        CategoryExistsByIdQuery query = new(req.Id);
        bool exists = await mediator.Send(query, ct).ConfigureAwait(false);
        
        if (!exists)
        {
            ValidationFailures.Add(new() 
            {
                ErrorMessage = string.Format(NotFound, "Category")
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        DeleteCategoryCommand command = new(req.Id);
        await mediator.Send(command, ct).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
