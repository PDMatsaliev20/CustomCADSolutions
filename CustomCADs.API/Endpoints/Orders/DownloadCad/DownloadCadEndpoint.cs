using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Orders.Queries.ExistsById;
using CustomCADs.Application.UseCases.Orders.Queries.GetCadById;
using CustomCADs.Application.UseCases.Orders.Queries.HasCadById;
using CustomCADs.Application.UseCases.Orders.Queries.IsBuyer;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Orders.DownloadCad
{
    using static ApiMessages;
    using static StatusCodes;

    public class DownloadCadEndpoint(IMediator mediator, IWebHostEnvironment env) : Endpoint<DownloadCadRequest, byte[]>
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
            OrderExistsByIdQuery existsQuery = new(req.Id);
            bool orderExists = await mediator.Send(existsQuery).ConfigureAwait(false);

            if (!orderExists)
            {
                await SendNotFoundAsync().ConfigureAwait(false);
                return;
            }

            OrderHasCadByIdQuery hasCadQuery = new(req.Id);
            bool orderHasCad = await mediator.Send(existsQuery).ConfigureAwait(false);

            if (!orderHasCad)
            {
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            IsOrderBuyerQuery isBuyerQuery = new(req.Id, User.GetName());
            bool userIsOrderBuyer = await mediator.Send(isBuyerQuery);

            if (!userIsOrderBuyer)
            {
                ValidationFailures.Add(new()
                {
                    ErrorMessage = ForbiddenAccess,
                });
                await SendErrorsAsync().ConfigureAwait(false);
                return;
            }

            GetOrderCadByIdQuery orderCadQuery = new(req.Id);
            CadModel model = await mediator.Send(orderCadQuery).ConfigureAwait(false);

            byte[] bytes = await env.GetCadBytes(model.Name + model.Id, model.Paths.FileExtension).ConfigureAwait(false);
            bool isGlb = model.Paths.FileExtension == ".glb";

            string fileName = model.Name + (isGlb ? ".glb" : ".zip");
            string contentType = isGlb ? "model/gltf-binary" : "application/zip";

            await SendBytesAsync(bytes, fileName, contentType).ConfigureAwait(false);
        }
    }
}
