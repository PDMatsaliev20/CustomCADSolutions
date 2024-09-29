using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.Services;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.DeleteCad
{
    using static StatusCodes;

    public class DeleteCadEndpoint(ICadService service, IWebHostEnvironment env) : Endpoint<DeleteCadRequest>
    {
        public override void Configure()
        {
            Delete("{id}");
            Group<CadsGroup>();
            Description(d => d.WithSummary("Deletes the Cad with the specified id."));
            Options(opt =>
            {
                opt.Produces(Status204NoContent);
            });
        }

        public override async Task HandleAsync(DeleteCadRequest req, CancellationToken ct)
        {
            CadModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

            if (model.CreatorId != User.GetId())
            {
                await SendForbiddenAsync().ConfigureAwait(false);
                return;
            }

            string cadFileName = model.Name + req.Id, cadExtension = model.Paths.FileExtension;
            string imageFileName = model.Name + req.Id, imageExtension = model.Paths.ImageExtension;

            await service.DeleteAsync(req.Id).ConfigureAwait(false);
            env.DeleteFile("images", imageFileName, imageExtension);
            env.DeleteFile("cads", cadFileName, cadExtension);

            await SendNoContentAsync().ConfigureAwait(false);
        }
    }
}
