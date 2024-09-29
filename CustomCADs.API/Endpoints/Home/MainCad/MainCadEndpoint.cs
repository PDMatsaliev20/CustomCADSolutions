using CustomCADs.API.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home.MainCad
{
    using static StatusCodes;

    public class CategoriesEndpoint : EndpointWithoutRequest<CadGetDTO>
    {
        public override void Configure()
        {
            Get("MainCad");
            Group<HomeGroup>();
            AllowAnonymous();
            Description(b =>
            {
                b.WithSummary("Gets the path and coordinates to the 3D Model for the Home Page.");
            });
            Options(opt =>
            {
                opt.Produces(Status200OK, typeof(CadGetDTO), "application/json");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            CadGetDTO cad = new()
            {
                CadPath = "/files/HomeCAD.glb",
                CamCoordinates = new(2, 16, 33),
                PanCoordinates = new(0, 6, -3),
            };

            await SendAsync(cad, Status200OK).ConfigureAwait(false);
        }
    }
}
