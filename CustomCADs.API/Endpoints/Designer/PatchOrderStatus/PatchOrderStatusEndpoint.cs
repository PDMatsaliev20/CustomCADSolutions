using CustomCADs.API.Helpers;
using CustomCADs.Application.UseCases.Orders.Commands.SetStatus;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Designer.PatchOrderStatus;

using static ApiMessages;
using static StatusCodes;

public class PatchOrderStatusEndpoint(IMediator mediator) : Endpoint<PatchOrderStatusRequest>
{
    private readonly string[] actions = ["begin", "report", "cancel", "finish"];

    public override void Configure()
    {
        Patch("Orders/{id}");
        Group<DesignerGroup>();
        Description(d => d
            .WithSummary("Updates the specified Order with the specified Status.")
            .Produces<EmptyResponse>(Status204NoContent)
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(PatchOrderStatusRequest req, CancellationToken ct)
    {
        string action = req.Action.ToLower();
        SetOrderStatusCommand? command = action switch
        {
            "begin" => new(req.Id, action, User.GetId()),
            "report" => new(req.Id, action),
            "cancel" => new(req.Id, action, User.GetId()),
            "finish" => new(req.Id, action, CadId: req.CadId),
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
