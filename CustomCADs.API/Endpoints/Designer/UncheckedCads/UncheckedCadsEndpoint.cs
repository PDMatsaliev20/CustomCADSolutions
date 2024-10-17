using CustomCADs.API.Dtos;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetAll;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCads;

using static StatusCodes;

public class UncheckedCadsEndpoint(IMediator mediator) : Endpoint<UncheckedCadsRequest, CadResultDto<UncheckedCadsResponse>>
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
        GetAllCadsQuery query = new(
            Category: req.Category,
            Status: nameof(CadStatus.Unchecked),
            Creator: req.Creator,
            Name: req.Name,
            Sorting: req.Sorting ?? string.Empty,
            Page: req.Page,
            Limit: req.Limit
        );
        CadResult result = await mediator.Send(query, ct).ConfigureAwait(false);

        CadResultDto<UncheckedCadsResponse> response = new()
        {
            Count = result.Count,
            Cads = result.Cads.Select(cad => cad.Adapt<UncheckedCadsResponse>()).ToArray() 
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
