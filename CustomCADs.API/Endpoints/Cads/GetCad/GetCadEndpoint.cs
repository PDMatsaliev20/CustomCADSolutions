using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using FastEndpoints;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Cads.GetCad
{
    using static StatusCodes;

    public class GetCadEndpoint(ICadService service) : Endpoint<GetCadRequest, GetCadResponse>
    {
        public override void Configure()
        {
            Get("{id}");
            Group<CadsGroup>();
            Description(d => d.WithSummary("Gets a Cad by the specified id."));
            Options(opt =>
            {
                opt.Produces<GetCadResponse>(Status200OK, "application/json");
            });
        }

        public override async Task HandleAsync(GetCadRequest req, CancellationToken ct)
        {
            CadModel model = await service.GetByIdAsync(req.Id).ConfigureAwait(false);

            if (model.CreatorId != User.GetId())
            {
                await SendForbiddenAsync().ConfigureAwait(false);
                return;
            }

            GetCadResponse response = new()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                CadPath = model.Paths.FilePath,
                ImagePath = model.Paths.ImagePath,
                CamCoordinates = new(model.CamCoordinates.X, model.CamCoordinates.Y, model.CamCoordinates.Z),
                PanCoordinates = new(model.PanCoordinates.X, model.PanCoordinates.Y, model.PanCoordinates.Z),
                OrdersCount = model.Orders.Count,
                CreationDate = model.CreationDate.ToString(DateFormatString),
                CreatorName = model.Creator.UserName,
                Status = model.Status.ToString(),
                Category = new(model.CategoryId, model.Category.Name),
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
