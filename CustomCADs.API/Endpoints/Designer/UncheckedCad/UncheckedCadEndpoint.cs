using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetCadAndAdjacentById;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Designer.UncheckedCad;

using static StatusCodes;

public class UncheckedCadEndpoint(IMediator mediator) : Endpoint<UncheckedCadRequest, UncheckedCadResponse>
{
    public override void Configure()
    {
        Get("Cads/{id}");
        Group<DesignerGroup>();
        Description(d => d
            .WithSummary("Gets the requested Cad, as well as the previous and next ones in line.")
            .Produces<UncheckedCadResponse>(Status200OK, "application/json")
            .ProducesProblem(Status404NotFound));
    }

    public override async Task HandleAsync(UncheckedCadRequest req, CancellationToken ct)
    {
        GetCadAndAdjacentByIdQuery query = new(req.Id);
        (int? prevId, CadModel cad, int? nextId) = await mediator.Send(query, ct).ConfigureAwait(false);

        var response = cad.Adapt<UncheckedCadResponse>();
        response.PrevId = prevId;
        response.NextId = nextId;

        await SendOkAsync(response).ConfigureAwait(false);
    }
}
