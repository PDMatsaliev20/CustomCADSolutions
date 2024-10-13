using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.PatchOrderStatus
{
    using static ApiMessages;
    using static StatusCodes;

    public class PatchOrderStatusEndpoint(IDesignerService service) : Endpoint<PatchOrderStatusRequest>
    {
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
            switch (req.Action.ToLower())
            {
                case "begin":
                    await service.BeginAsync(req.Id, User.GetName()).ConfigureAwait(false);
                    break;

                case "report":
                    await service.ReportAsync(req.Id).ConfigureAwait(false);
                    break;

                case "cancel":
                    await service.CancelAsync(req.Id, User.GetName()).ConfigureAwait(false);
                    break;

                case "complete":
                case "finish":
                    int cadId = req.CadId ?? throw new ArgumentNullException();
                    await service.CompleteAsync(req.Id, cadId, User.GetName()).ConfigureAwait(false);
                    break;

                default:
                    string[] actions = ["begin", "report", "cancel", "complete", "finish"];                    
                    ValidationFailures.Add(new()
                    {
                        PropertyName = nameof(req.Action),
                        AttemptedValue = req.Action,
                        ErrorMessage = string.Format(InvalidAction, string.Join(", ", actions)),
                    });

                    await SendErrorsAsync().ConfigureAwait(false);
                    return;
            }
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
