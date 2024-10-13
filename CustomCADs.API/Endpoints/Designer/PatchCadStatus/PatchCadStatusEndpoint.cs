using CustomCADs.Application.Contracts;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.PatchCadStatus
{
    using static StatusCodes;

    public class PatchCadStatusEndpoint(IDesignerService service) : Endpoint<PatchCadStatusRequest>
    {
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
            CadStatus status = Enum.Parse<CadStatus>(req.Status);
            await service.EditCadStatusAsync(req.Id, status).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
