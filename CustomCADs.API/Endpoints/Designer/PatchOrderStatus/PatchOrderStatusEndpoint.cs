using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Domain.Enums;
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
            Description(d => d.WithSummary("Updates the specified Cad with the specified Status."));
            Options(opt =>
            {
                opt.Produces<EmptyResponse>(Status204NoContent);
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(PatchOrderStatusRequest req, CancellationToken ct)
        {
            switch (req.Status.ToLower())
            {
                case "begin": 
                    await service.BeginAsync(req.Id, User.GetId()).ConfigureAwait(false); 
                    break;

                case "report": 
                    await service.ReportAsync(req.Id).ConfigureAwait(false); 
                    break;

                case "cancel": 
                    await service.CancelAsync(req.Id, User.GetId()).ConfigureAwait(false);
                    break;

                case "complete": 
                case "finish":
                    int cadId = req.CadId ?? throw new ArgumentNullException();
                    await service.CompleteAsync(req.Id, cadId, User.GetId()).ConfigureAwait(false); 
                    break;

                default:
                    string[] statuses = Enum.GetNames<CadStatus>();
                    string message = string.Format(InvalidStatus, string.Join(", ", statuses));
                    await SendResultAsync(Results.BadRequest(message)).ConfigureAwait(false);
                    return;
            }
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
