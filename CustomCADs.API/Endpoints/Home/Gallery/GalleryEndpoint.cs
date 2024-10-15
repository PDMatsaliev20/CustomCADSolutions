using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetAll;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Home.Gallery;

using static StatusCodes;

public class GalleryEndpoint(IMediator mediator) : Endpoint<GalleryRequest, CadResultDto<GalleryResponse>>
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
        GetAllCadsQuery query = new(
                Status: nameof(CadStatus.Validated),
                Category: req.Category,
                Name: req.Name,
                Creator: req.Creator,
                Sorting: req.Sorting ?? string.Empty,
                Page: req.Page,
                Limit: req.Limit
        );
        CadResult result = await mediator.Send(query).ConfigureAwait(false);

        CadResultDto<GalleryResponse> response = new()
        {
            Count = result.Count,
            Cads = result.Cads.Select(cad => cad.Adapt<GalleryResponse>()).ToArray(),
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
