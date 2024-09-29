using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.GetCads
{
    using static StatusCodes;

    public class GetCadsEndpoint(ICadService service) : Endpoint<GetCadsRequest, CadQueryResultDTO>
    {
        public override void Configure()
        {
            Get("");
            Group<CadsGroup>();
            Description(d => d.WithSummary("Queries the User's Cads with the specified parameters."));
            Options(opt =>
            {
                opt.Produces<CadQueryResultDTO>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(GetCadsRequest req, CancellationToken ct)
        {
            CadResult result = service.GetAllAsync(
                    creator: User.Identity?.Name,
                    category: req.Category,
                    name: req.Name,
                    sorting: req.Sorting ?? string.Empty,
                    page: req.Page,
                    limit: req.Limit
                );

            CadQueryResultDTO response = new()
            {
                Count = result.Count,
                Cads = result.Cads
                    .Select(cad => new CadGetDTO() 
                    {
                        Id = cad.Id,
                        Name = cad.Name,
                        ImagePath = cad.Paths.ImagePath,
                        CreationDate = cad.CreationDate.ToString("dd-MM-yyyy HH:mm:ss"),
                        CreatorName = cad.Creator.UserName,
                        Status = cad.Status.ToString(),
                        Category = new()
                        {
                            Id = cad.Category.Id,
                            Name = cad.Category.Name,
                        },
                    }).ToArray()
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
