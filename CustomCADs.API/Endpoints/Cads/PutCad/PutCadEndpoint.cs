using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Commands.Edit;
using CustomCADs.Application.UseCases.Cads.Commands.SetPaths;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.PutCad;

using static ApiMessages;
using static StatusCodes;

public class PutCadEndpoint(IMediator mediator, IWebHostEnvironment env) : Endpoint<PutCadRequest>
{
    public override void Configure()
    {
        Put("{id}");
        Group<CadsGroup>();
        Description(d => d
            .WithSummary("Updates Name, Description, Price CategoryId and optionally Image properties of Cad.")
            .Accepts<PutCadRequest>("multipart/form-data")
            .Produces<EmptyResponse>(Status204NoContent));
    }

    public override async Task HandleAsync(PutCadRequest req, CancellationToken ct)
    {
        GetCadByIdQuery query = new(req.Id);
        CadModel cad = await mediator.Send(query).ConfigureAwait(false);

        if (cad.CreatorId != User.GetId())
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = ForbiddenAccess,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        if (req.Image != null)
        {
            env.DeleteFile("images", cad.Name + cad.Id, cad.Paths.ImageExtension);
            string imagePath = await env.UploadImageAsync(req.Image, req.Name + req.Id + req.Image.GetFileExtension()).ConfigureAwait(false);

            SetCadPathsCommand setPathsCommand = new(cad.Id, cad.Paths.FilePath, imagePath);
            await mediator.Send(setPathsCommand).ConfigureAwait(false);
        }

        cad.Name = req.Name;
        cad.Description = req.Description;
        cad.CategoryId = req.CategoryId;
        cad.Price = req.Price;

        EditCadCommand editCommand = new(req.Id, cad);
        await mediator.Send(editCommand).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
