using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Cads.GetCads
{
    using static StatusCodes;

    public class GetCadsEndpoint(ICadService service) : Endpoint<GetCadsRequest, CadResultDto<GetCadsResponse>>
    {
        public override void Configure()
        {
            Get("");
            Group<CadsGroup>();
            Description(d => d
                .WithSummary("Queries the User's Cads with the specified parameters.")
                .Produces<CadResultDto<GetCadsResponse>>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(GetCadsRequest req, CancellationToken ct)
        {
            CadResult result = service.GetAllAsync(
                creator: User.GetName(),
                category: req.Category,
                name: req.Name,
                sorting: req.Sorting ?? string.Empty,
                page: req.Page,
                limit: req.Limit
            );

            CadResultDto<GetCadsResponse> response = new()
            {
                Count = result.Count,
                Cads = result.Cads.Select(cad => cad.Adapt<GetCadsResponse>()).ToArray()
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
