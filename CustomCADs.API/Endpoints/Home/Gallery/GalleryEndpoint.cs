using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home.Gallery
{
    using static StatusCodes;

    public class GalleryEndpoint(ICadService service) : Endpoint<GalleryRequest, GalleryResponse>
    {
        public override void Configure()
        {
            Get("Gallery");
            Group<HomeGroup>();
            Description(d => d.WithSummary("Queries all Validated 3D Models with the specified parameters."));
            Options(opt =>
            {
                opt.Produces<GalleryResponse>(Status200OK, "application/json");
                opt.ProducesProblem(Status500InternalServerError);
            });
        }

        public override async Task HandleAsync(GalleryRequest req, CancellationToken ct)
        {
            CadResult result = service.GetAllAsync(
                    status: nameof(CadStatus.Validated),
                    category: req.Category,
                    name: req.Name,
                    owner: req.Creator,
                    sorting: req.Sorting ?? string.Empty,
                    page: req.Page,
                    limit: req.Limit
            );

            GalleryResponse response = new()
            {
                Count = result.Count,
                Cads = result.Cads
                    .Select(c => new GalleryCadDto()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CreationDate = c.CreationDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        CreatorName = c.Creator.UserName,
                        ImagePath = c.Paths.ImagePath,
                    }).ToArray(),
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
