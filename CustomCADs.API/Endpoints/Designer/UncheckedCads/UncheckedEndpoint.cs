using CustomCADs.API.Models.Cads;
using CustomCADs.API.Models.Queries;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCads
{
    using static StatusCodes;

    public class UncheckedEndpoint(IDesignerService service) : Endpoint<UncheckedRequest, CadQueryResultDTO>
    {
        public override void Configure()
        {
            Get("Cads");
            Group<DesignerGroup>();
            Description(d => d.WithSummary("Gets all Cads with Unchecked status."));
            Options(opt =>
            {
                opt.Produces<CadQueryResultDTO>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(UncheckedRequest req, CancellationToken ct)
        {
            CadResult result = service.GetCadsAsync(
                category: req.Category,
                creator: req.Creator,
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
                    }).ToArray(),
            };

            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
