using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.PutCad
{
    using static ApiMessages;
    using static StatusCodes;

    public class PutCadEndpoint(ICadService service, IWebHostEnvironment env) : Endpoint<PutCadRequest>
    {
        public override void Configure()
        {
            Put("{id}");
            Group<CadsGroup>();
            Description(d => d
                .WithSummary("Updates Name, Description, Price CategoryId and optionally Image properties of Cad.")
                .Accepts<PutCadRequest>("multipart/form-data")
                .Produces<EmptyResponse>(Status204NoContent));
        }

        public override async Task HandleAsync(PutCadRequest req, CancellationToken ct)
        {
            CadModel cad = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

            if (cad.CreatorId != User.GetId())
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            if (req.Image != null)
            {
                env.DeleteFile("images", cad.Name + cad.Id, cad.Paths.ImageExtension);
                string imagePath = await env.UploadImageAsync(req.Image, req.Name + req.Id + req.Image.GetFileExtension()).ConfigureAwait(false);
                await service.SetPathsAsync(req.Id, cad.Paths.FilePath, imagePath).ConfigureAwait(false);
            }

            cad.Name = req.Name;
            cad.Description = req.Description;
            cad.CategoryId = req.CategoryId;
            cad.Price = req.Price;
            await service.EditAsync(req.Id, cad).ConfigureAwait(false);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
