using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home.MainCad
{
    using static StatusCodes;

    public class MainCadEndpoint : EndpointWithoutRequest<MainCadResponse>
    {
        public override void Configure()
        {
            Get("MainCad");
            Group<HomeGroup>();
            AllowAnonymous();
            Description(d => d
                .WithSummary("Gets the path and coordinates to the 3D Model for the Home Page.")
                .Produces<MainCadResponse>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            MainCadResponse cad = new()
            {
                CadPath = "/files/HomeCAD.glb",
                CamCoordinates = new(2, 16, 33),
                PanCoordinates = new(0, 6, -3),
            };

            await SendAsync(cad, Status200OK).ConfigureAwait(false);
        }
    }
}
