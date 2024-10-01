using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home.GalleryDetails
{
    using static StatusCodes;

    public class GalleryDetailsEndpoint(ICadService service) : Endpoint<GalleryDetailsRequest, GalleryDetailsResponse>
    {
        public override void Configure()
        {
            Get("Gallery/{id}");
            Group<HomeGroup>();
            Description(d => d.WithSummary("Get info about a 3D Model from the Gallery."));
            Options(opt =>
            {
                opt.Produces<GalleryDetailsResponse>(Status200OK, "application/json");
                opt.ProducesProblem(Status404NotFound);
                opt.ProducesProblem(Status500InternalServerError);
            });
        }

        public override async Task HandleAsync(GalleryDetailsRequest req, CancellationToken ct)
        {
            CadModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

            GalleryDetailsResponse response = new() 
            {
                Id = model.Id,
                Name = model.Name,
                CadPath = model.Paths.FilePath,
                Price = model.Price,
                CreatorName = model.Creator.UserName,
                CreationDate = model.CreationDate.ToString("dd-MM-yyyy HH:mm:ss"),
                CamCoordinates = new(model.CamCoordinates.X, model.CamCoordinates.Y, model.CamCoordinates.X),
                PanCoordinates = new(model.PanCoordinates.X, model.PanCoordinates.Y, model.PanCoordinates.X),
                Category = new(model.CategoryId, model.Category.Name),
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}

/*
 */