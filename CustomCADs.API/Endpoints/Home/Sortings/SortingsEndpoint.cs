using CustomCADs.Domain.Shared.Enums;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Home.Sortings;

using static StatusCodes;

public class SortingsEndpoint : EndpointWithoutRequest<string[]>
{
    public override void Configure()
    {
        Get("/Sortings");
        Group<HomeGroup>();
        Description(d => d
            .WithSummary("Gets all existing Sortings.")
            .Produces<string[]>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string[] sortings = Enum.GetNames<Sorting>();
        await SendOkAsync(sortings).ConfigureAwait(false);
    }
}
