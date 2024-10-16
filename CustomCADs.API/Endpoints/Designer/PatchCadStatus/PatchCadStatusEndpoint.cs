using CustomCADs.Application.UseCases.Cads.Commands.SetStatus;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Designer.PatchCadStatus;

using static ApiMessages;
using static StatusCodes;

public class PatchCadStatusEndpoint(IMediator mediator) : Endpoint<PatchCadStatusRequest>
{
    private readonly string[] actions = ["validate", "report"];

    public override void Configure()
    {
        Patch("Cads/{id}");
        Group<DesignerGroup>();
        Description(d => d
            .WithSummary("Updates the specified Cad with the specified Status.")
            .Produces<EmptyResponse>(Status204NoContent)
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(PatchCadStatusRequest req, CancellationToken ct)
    {
        string action = req.Action.ToLower();
        SetCadStatusCommand? command = action switch
        {
            "validate" => new(req.Id, action),
            "report" => new(req.Id, action),
            _ => null,
        };

        if (command == null)
        {
            ValidationFailures.Add(new()
            {
                PropertyName = nameof(req.Action),
                AttemptedValue = req.Action,
                ErrorMessage = string.Format(InvalidAction, string.Join(", ", actions)),
            });

            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }
        await mediator.Send(command).ConfigureAwait(false);

        await SendNoContentAsync().ConfigureAwait(false);
    }
}
