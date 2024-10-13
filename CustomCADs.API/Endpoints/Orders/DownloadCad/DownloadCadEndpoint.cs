using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Orders.DownloadCad
{
    using static ApiMessages;
    using static StatusCodes;

    public class DownloadCadEndpoint(IOrderService service, IWebHostEnvironment env) : Endpoint<DownloadCadRequest, byte[]>
    {
        public override void Configure()
        {
            Get("{id}/DownloadCad");
            Group<OrdersGroup>();
            Description(d => d
                .WithSummary("Downloads the Cad of the Order with the specified id, as a .glb or a .zip depending on the way it was uploaded.")
                .Produces<byte[]>(Status200OK, "model/gltf-binary")
                .Produces<byte[]>(Status200OK, "application/zip"));
        }

        public override async Task HandleAsync(DownloadCadRequest req, CancellationToken ct)
        {
            bool orderExists = await service.ExistsByIdAsync(req.Id).ConfigureAwait(false);
            if (!orderExists)
            {
                await SendNotFoundAsync().ConfigureAwait(false);
                return;
            }

            bool orderHasCad = await service.HasCadAsync(req.Id).ConfigureAwait(false);
            if (!orderHasCad)
            {
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            bool userOwnsOrder = await service.CheckOwnership(req.Id, User.GetName());
            if (!userOwnsOrder)
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            CadModel model = await service.GetCadAsync(req.Id).ConfigureAwait(false);

            byte[] bytes = await env.GetCadBytes(model.Name + model.Id, model.Paths.FileExtension).ConfigureAwait(false);
            bool isGlb = model.Paths.FileExtension == ".glb";

            string fileName = model.Name + (isGlb ? ".glb" : ".zip");
            string contentType = isGlb ? "model/gltf-binary" : "application/zip";

            await SendBytesAsync(bytes, fileName, contentType).ConfigureAwait(false);
        }
    }
}
