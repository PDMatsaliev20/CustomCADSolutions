using CustomCADs.API.Dtos;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetAll;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.GetCads;

using static StatusCodes;

public class GetCadsEndpoint(IMediator mediator) : Endpoint<GetCadsRequest, CadResultDto<GetCadsResponse>>
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
        GetAllCadsQuery query = new(
            Creator: User.GetName(),
            Category: req.Category,
            Name: req.Name,
            Sorting: req.Sorting ?? string.Empty,
            Page: req.Page,
            Limit: req.Limit
        );
        CadResult result = await mediator.Send(query, ct).ConfigureAwait(false);
        
        CadResultDto<GetCadsResponse> response = new()
        {
            Count = result.Count,
            Cads = result.Cads.Select(cad => cad.Adapt<GetCadsResponse>()).ToArray()
        };
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
