using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCad
{
    using static StatusCodes;

    public class UncheckedCadEndpoint(IDesignerService service) : Endpoint<UncheckedCadRequest, UncheckedCadResponse>
    {
        public override void Configure()
        {
            Get("Cads/{id}");
            Group<DesignerGroup>();
            Description(d => d
                .WithSummary("Gets the requested Cad, as well as the previous and next ones in line.")
                .Produces<UncheckedCadResponse>(Status200OK, "application/json")
                .ProducesProblem(Status404NotFound));
        }

        public override async Task HandleAsync(UncheckedCadRequest req, CancellationToken ct)
        {
            (int? prevId, CadModel cad, int? nextId) = await service
                .GetNextCurrentAndPreviousByIdAsync(req.Id)
                .ConfigureAwait(false);

            UncheckedCadResponse response = cad.Adapt<UncheckedCadResponse>();
            response.PrevId = prevId;
            response.NextId = nextId;

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
