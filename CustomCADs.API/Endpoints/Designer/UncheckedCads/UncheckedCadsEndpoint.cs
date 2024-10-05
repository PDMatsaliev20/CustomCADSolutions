using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCads
{
    using static StatusCodes;

    public class UncheckedCadsEndpoint(IDesignerService service) : Endpoint<UncheckedCadsRequest, CadResultDto<UncheckedCadsResponse>>
    {
        public override void Configure()
        {
            Get("Cads");
            Group<DesignerGroup>();
            Description(d => d
                .WithSummary("Gets all Cads with Unchecked status.")
                .Produces<CadResultDto<UncheckedCadsResponse>>(Status200OK, "application/json"));
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
                Cads = result.Cads.Select(cad => cad.Adapt<UncheckedCadsResponse>()).ToArray() 
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
