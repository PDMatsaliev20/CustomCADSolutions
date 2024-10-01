using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCads
{
    using static StatusCodes;

    public class UncheckedCadsEndpoint(IDesignerService service) : Endpoint<UncheckedCadsRequest, CadResultDto<UncheckedCadsResponse>>
    {
        public override void Configure()
        {
            Get("Cads");
            Group<DesignerGroup>();
            Description(d => d.WithSummary("Gets all Cads with Unchecked status."));
            Options(opt =>
            {
                opt.Produces<UncheckedCadsResponse>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(UncheckedCadsRequest req, CancellationToken ct)
        {
            CadResult result = service.GetCadsAsync(
                category: req.Category,
                creator: req.Creator,
                name: req.Name,
                sorting: req.Sorting ?? string.Empty,
                page: req.Page,
                limit: req.Limit
            );

            CadResultDto<UncheckedCadsResponse> response = new()
            {
                Count = result.Count,
                Cads = result.Cads
                    .Select(cad => new UncheckedCadsResponse()
                    {
                        Id = cad.Id,
                        Name = cad.Name,
                        ImagePath = cad.Paths.ImagePath,
                        CreationDate = cad.CreationDate.ToString(DateFormatString),
                        CreatorName = cad.Creator.UserName,
                        Status = cad.Status.ToString(),
                        Category = new(cad.CategoryId, cad.Category.Name),
                    }).ToArray(),
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
