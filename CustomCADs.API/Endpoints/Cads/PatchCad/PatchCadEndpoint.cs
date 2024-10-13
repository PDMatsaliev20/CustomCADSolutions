using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using FluentValidation.Results;

namespace CustomCADs.API.Endpoints.Cads.PatchCad
{
    using static ApiMessages;
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
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

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

            bool isValid = model.Validate(out IList<string> errors);
            if (!isValid)
            {
                var failures = errors.Select(e => new ValidationFailure() { ErrorMessage = e });
                ValidationFailures.AddRange(failures);
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            await service.EditAsync(req.Id, model).ConfigureAwait(false);
            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
