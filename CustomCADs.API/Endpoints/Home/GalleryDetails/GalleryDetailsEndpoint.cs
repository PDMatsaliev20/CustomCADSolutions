using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Home.GalleryDetails
{
    using static StatusCodes;

    public class GalleryDetailsEndpoint(ICadService service) : Endpoint<GalleryDetailsRequest, GalleryDetailsResponse>
    {
        public override void Configure()
        {
            Get("Gallery/{id}");
            Group<HomeGroup>();
            Description(d => d
                .WithSummary("Get info about a 3D Model from the Gallery.")
                .Produces<GalleryDetailsResponse>(Status200OK, "application/json")
                .ProducesProblem(Status404NotFound)
                .ProducesProblem(Status500InternalServerError));
        }

        public override async Task HandleAsync(GalleryDetailsRequest req, CancellationToken ct)
        {
            CadModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);
            GalleryDetailsResponse response = model.Adapt<GalleryDetailsResponse>();
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
