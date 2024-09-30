using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCad
{
    using static StatusCodes;

    public class UncheckedCadEndpoint(IDesignerService service) : Endpoint<UncheckedCadRequest, UncheckedCadResponse>
    {
        public override void Configure()
        {
            Get("Cads/{id}");
            Group<DesignerGroup>();
            Description(d => d.WithSummary("Gets the requested Cad, as well as the previous and next ones in line."));
            Options(opt =>
            {
                opt.Produces<UncheckedCadResponse>(Status200OK, "application/json");
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(UncheckedCadRequest req, CancellationToken ct)
        {
            (int? prevId, CadModel cad, int? nextId) = await service
                .GetNextCurrentAndPreviousByIdAsync(req.Id)
                .ConfigureAwait(false);

            UncheckedCadResponse response = new()
            {
                PrevId = prevId,
                Cad = new()
                {
                    Id = cad.Id,
                    CadPath = cad.Paths.FilePath,
                    CamCoordinates = new(cad.CamCoordinates.X, cad.CamCoordinates.Y, cad.CamCoordinates.Z),
                    PanCoordinates = new(cad.PanCoordinates.X, cad.PanCoordinates.Y, cad.PanCoordinates.Z),
                },
                NextId = nextId,
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
