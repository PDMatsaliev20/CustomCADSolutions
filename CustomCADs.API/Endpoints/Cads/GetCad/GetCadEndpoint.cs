using CustomCADs.API.Helpers;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Application.UseCases.Cads.Queries.GetById;
using FastEndpoints;
using Mapster;
using MediatR;

namespace CustomCADs.API.Endpoints.Cads.GetCad;

using static ApiMessages;
using static StatusCodes;

public class GetCadEndpoint(IMediator mediator) : Endpoint<GetCadRequest, GetCadResponse>
{
    public override void Configure()
    {
        Get("{id}");
        Group<CadsGroup>();
        Description(d => d
            .WithSummary("Gets a Cad by the specified id.")
            .Produces<GetCadResponse>(Status200OK, "application/json"));
    }

    public override async Task HandleAsync(GetCadRequest req, CancellationToken ct)
    {
        GetCadByIdQuery query = new(req.Id);
        CadModel model = await mediator.Send(query).ConfigureAwait(false);

        if (model.CreatorId != User.GetId())
        {
            ValidationFailures.Add(new()
            {
                ErrorMessage = ForbiddenAccess,
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        GetCadResponse response = model.Adapt<GetCadResponse>();
        await SendOkAsync(response).ConfigureAwait(false);
    }
}
