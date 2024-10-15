using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetAll;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.RecentCads;

using static StatusCodes;

public class RecentCadsEndpoint(IMediator mediator) : Endpoint<RecentCadsRequest, IEnumerable<RecentCadsResponse>>
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
        GetAllCadsQuery query = new(
            Creator: User.GetName(),
            Sorting: nameof(Sorting.Newest),
            Limit: req.Limit
        );
        CadResult result = await mediator.Send(query).ConfigureAwait(false);

        var response = result.Cads.Select(cad => cad.Adapt<RecentCadsResponse>());
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
