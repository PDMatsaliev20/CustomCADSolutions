using CustomCADs.API.Helpers;
using CustomCADs.API.Models.Cads;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Entities;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Cads.GetCad
{
    using static StatusCodes;

    public class GetCadEndpoint(ICadService service) : Endpoint<GetCadRequest, CadGetDTO>
    {
        public override void Configure()
        {
            Get("{id}");
            Group<CadsGroup>();
            Description(d => d.WithSummary("Gets a Cad by the specified id."));
            Options(opt =>
            {
                opt.Produces<CadGetDTO>(Status200OK, "application/json");
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

            CadGetDTO response = new()
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                CadPath = model.Paths.FilePath,
                CamCoordinates = new(model.CamCoordinates.X, model.CamCoordinates.Y, model.CamCoordinates.Z),
                PanCoordinates = new(model.PanCoordinates.X, model.PanCoordinates.Y, model.PanCoordinates.Z),
                Price = model.Price,
                ImagePath = model.Paths.ImagePath,
                OrdersCount = model.Orders.Count,
                CreationDate = model.CreationDate.ToString("dd-MM-yyyy HH:mm:ss"),
                CreatorName = model.Creator.UserName,
                Status = model.Status.ToString(),
                Category = new()
                {
                    Id = model.Category.Id,
                    Name = model.Category.Name,
                },
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
