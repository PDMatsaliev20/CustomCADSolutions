using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Home.GalleryDetails
{
    using static StatusCodes;

    public class GalleryDetailsEndpoint(IMediator mediator) : Endpoint<GalleryDetailsRequest, GalleryDetailsResponse>
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
            GetCadByIdQuery query = new(req.Id);
            CadModel model = await mediator.Send(query).ConfigureAwait(false);

            GalleryDetailsResponse response = model.Adapt<GalleryDetailsResponse>();
            await SendOkAsync(response).ConfigureAwait(false);
        }
    }
}
