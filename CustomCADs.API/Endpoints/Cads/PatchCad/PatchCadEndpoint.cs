using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Commands.Edit;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using CustomCADs.Application.UseCases.Cads.Queries.IsCreator;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.PatchCad;

using static ApiMessages;
using static StatusCodes;

public class PatchCadEndpoint(IMediator mediator) : Endpoint<PatchCadRequest>
{
    public override void Configure()
    {
        Patch("{id}");
        Group<CadsGroup>();
        Description(d => d
            .WithSummary("Updates CamCoordinates or PanCoordinates property of Cad.")
            .Accepts<PatchCadRequest>("application/json")
            .Produces<EmptyResponse>(Status200OK));
    }

    public override async Task HandleAsync(PatchCadRequest req, CancellationToken ct)
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

        double x = req.Coordinates.X, y = req.Coordinates.Y, z = req.Coordinates.Z;
        switch (req.Type.ToLower())
        {
            case "camera": model.CamCoordinates = new(x, y, z); break;
            case "pan": model.PanCoordinates = new(x, y, z); break;
            default:
                ValidationFailures.Add(new()
                {
                    PropertyName = nameof(req.Type),
                    AttemptedValue = req.Type,
                    ErrorMessage = "Type property must be either 'camera' or 'pan'",
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
        }

        EditCadCommand command = new(req.Id, model);
        await mediator.Send(command, ct).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
