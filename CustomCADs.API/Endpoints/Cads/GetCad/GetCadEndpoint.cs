using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Cads.GetCad
{
    using static ApiMessages;
    using static StatusCodes;

    public class GetCadEndpoint(ICadService service) : Endpoint<GetCadRequest, GetCadResponse>
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
            CadModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

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
}
