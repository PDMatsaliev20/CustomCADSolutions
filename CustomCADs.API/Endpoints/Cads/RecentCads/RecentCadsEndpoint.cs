using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Cads.RecentCads
{
    using static StatusCodes;

    public class RecentCadsEndpoint(ICadService service) : Endpoint<RecentCadsRequest, IEnumerable<RecentCadsResponse>>
    {
        public override void Configure()
        {
            Get("Recent");
            Group<CadsGroup>();
            Description(d => d
                .WithSummary("Gets the User's most recent Cads.")
                .Produces<IEnumerable<RecentCadsResponse>>(Status200OK, "application/json"));
        }

        public override async Task HandleAsync(RecentCadsRequest req, CancellationToken ct)
        {
            CadResult result = service.GetAllAsync(
                creator: User.GetName(),
                sorting: nameof(Sorting.Newest),
                limit: req.Limit
            );

            var response = result.Cads.Select(cad => cad.Adapt<RecentCadsResponse>());
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
