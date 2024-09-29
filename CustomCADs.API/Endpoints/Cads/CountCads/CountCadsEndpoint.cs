using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.CountCads
{
    using static StatusCodes;

    public class CountCadsEndpoint(ICadService service) : EndpointWithoutRequest<CountCadsResponse>
    {
        public override void Configure()
        {
            Get("Count");
            Group<CadsGroup>();
            Description(d => d.WithSummary("Gets counts of the User's Cads grouped by their status."));
            Options(opt =>
            {
                opt.Produces<CountCadsResponse>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            bool predicate(CadModel cad, CadStatus s)
                    => cad.Status == s && cad.Creator.UserName == User.Identity!.Name;

            int uncheckedCadsCounts = await service.Count(c => predicate(c, CadStatus.Unchecked)).ConfigureAwait(false);
            int validatedCadsCounts = await service.Count(c => predicate(c, CadStatus.Validated)).ConfigureAwait(false);
            int reportedCadsCounts = await service.Count(c => predicate(c, CadStatus.Reported)).ConfigureAwait(false);
            int bannedCadsCounts = await service.Count(c => predicate(c, CadStatus.Banned)).ConfigureAwait(false);

            CountCadsResponse response = new(uncheckedCadsCounts, validatedCadsCounts, reportedCadsCounts, bannedCadsCounts);
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
