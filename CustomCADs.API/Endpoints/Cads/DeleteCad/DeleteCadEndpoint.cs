using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Commands.Delete;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using CustomCADs.Application.UseCases.Cads.Queries.IsCreator;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.DeleteCad;

using static ApiMessages;
using static StatusCodes;

public class DeleteCadEndpoint(IMediator mediator, IWebHostEnvironment env) : Endpoint<DeleteCadRequest>
{
    public override void Configure()
    {
        Delete("{id}");
        Group<CadsGroup>();
        Description(d => d
            .WithSummary("Deletes the Cad with the specified id.")
            .Produces(Status204NoContent));
    }

    public override async Task HandleAsync(DeleteCadRequest req, CancellationToken ct)
    {
        IsCadCreatorQuery isCreatorQuery = new(req.Id, User.GetName());
        bool userIsCreator = await mediator.Send(isCreatorQuery, ct).ConfigureAwait(false);

        if (userIsCreator)
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = ForbiddenAccess,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        GetCadByIdQuery getCadQuery = new(req.Id);
        CadModel model = await mediator.Send(getCadQuery, ct).ConfigureAwait(false);

        string cadFileName = model.Name + req.Id, cadExtension = model.Paths.FileExtension;
        string imageFileName = model.Name + req.Id, imageExtension = model.Paths.ImageExtension;

        DeleteCadCommand command = new(req.Id);
        await mediator.Send(command, ct).ConfigureAwait(false);
        
        env.DeleteFile("images", imageFileName, imageExtension);
        env.DeleteFile("cads", cadFileName, cadExtension);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
