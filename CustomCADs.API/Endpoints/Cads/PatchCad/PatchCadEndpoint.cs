using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.PatchCad
{
    using static StatusCodes;

    public class PatchCadEndpoint(ICadService service) : Endpoint<PatchCadRequest>
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
            CadModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

            if (model.CreatorId != User.GetId())
            {
                await SendForbiddenAsync().ConfigureAwait(false);
                return;
            }

            double x = req.Coordinates.X, y = req.Coordinates.Y, z = req.Coordinates.Z;
            switch (req.Type.ToLower())
            {
                case "camera": model.CamCoordinates = new(x, y, z); break;
                case "pan": model.PanCoordinates = new(x, y, z); break;
                default: await SendErrorsAsync().ConfigureAwait(false); return;
            }

            bool isValid = model.Validate(out IList<string> errors);
            if (!isValid)
            {
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            await service.EditAsync(req.Id, model).ConfigureAwait(false);
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
