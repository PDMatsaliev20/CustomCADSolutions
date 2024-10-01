using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Home.Gallery
{
    using static StatusCodes;

    public class GalleryEndpoint(ICadService service) : Endpoint<GalleryRequest, CadResultDto<GalleryResponse>>
    {
        public override void Configure()
        {
            Get("Gallery");
            Group<HomeGroup>();
            Description(d => d
                .WithSummary("Queries all Validated 3D Models with the specified parameters.")
                .Produces<CadResultDto<GalleryResponse>>(Status200OK, "application/json")
                .ProducesProblem(Status500InternalServerError));
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

            CadResultDto<GalleryResponse> response = new()
            {
                Count = result.Count,
                Cads = result.Cads
                    .Select(c => new GalleryResponse()
                    {
                        Id = c.Id,
                        Name = c.Name,
                        CreationDate = c.CreationDate.ToString(DateFormatString),
                        CreatorName = c.Creator.UserName,
                        ImagePath = c.Paths.ImagePath,
                    }).ToArray(),
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
