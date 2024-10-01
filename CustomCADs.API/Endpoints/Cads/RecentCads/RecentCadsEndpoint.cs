using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.RecentCads
{
    using static StatusCodes;

    public class RecentCadsEndpoint(ICadService service) : Endpoint<RecentCadsRequest, CadResultDto<RecentCadsResponse>>
    {
        public override void Configure()
        {
            Get("Recent");
            Group<CadsGroup>();
            Description(d => d.WithSummary("Gets the User's most recent Cads."));
            Options(opt =>
            {
                opt.Produces<CadResultDto<RecentCadsResponse>>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(RecentCadsRequest req, CancellationToken ct)
        {
            CadResult result = service.GetAllAsync(
                    creator: User.GetName(),
                    sorting: nameof(Sorting.Newest),
                    limit: req.Limit
                    );

            CadResultDto<RecentCadsResponse> response = new()
            {
                Count = result.Count,
                Cads = result.Cads
                    .Select(cad => new RecentCadsResponse()
                    {
                        Id = cad.Id,
                        Name = cad.Name,
                        CreationDate = cad.CreationDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        CreatorName = cad.Creator.UserName,
                        Status = cad.Status.ToString(),
                        Category = new(cad.CategoryId, cad.Category.Name),
                    }).ToArray()
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
