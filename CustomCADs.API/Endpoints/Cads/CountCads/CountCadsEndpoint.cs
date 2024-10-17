using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.Count;
using CustomCADs.Domain.Cads.Enums;
using FastEndpoints;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.CountCads;

using static StatusCodes;

public class CountCadsEndpoint(IMediator mediator) : EndpointWithoutRequest<CountCadsResponse>
{
    public override void Configure()
    {
        Get("Counts");
        Group<CadsGroup>();
        Description(d => d
            .WithSummary("Gets counts of the User's Cads grouped by their status.")
            .Produces<CountCadsResponse>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        CadsCountQuery query;
        bool predicate(CadModel c, CadStatus s) 
            => c.Status == s && c.Creator.UserName == User.GetName();
        
        query = new(c => predicate(c, CadStatus.Unchecked));
        int uncheckedCadsCounts = await mediator.Send(query, ct).ConfigureAwait(false);
        
        query = new(c => predicate(c, CadStatus.Validated));
        int validatedCadsCounts = await mediator.Send(query, ct).ConfigureAwait(false);
        
        query = new(c => predicate(c, CadStatus.Reported));
        int reportedCadsCounts = await mediator.Send(query, ct).ConfigureAwait(false);

        query = new(c => predicate(c, CadStatus.Banned));
        int bannedCadsCounts = await mediator.Send(query, ct).ConfigureAwait(false);

        CountCadsResponse response = new(uncheckedCadsCounts, validatedCadsCounts, reportedCadsCounts, bannedCadsCounts);
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
