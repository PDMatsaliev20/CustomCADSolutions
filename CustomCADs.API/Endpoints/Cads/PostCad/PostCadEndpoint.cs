using CustomCADs.API.Endpoints.Cads.GetCad;
using CustomCADs.API.Helpers;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Cads;
using CustomCADs.Domain.Enums;
using FastEndpoints;
using Mapster;
using static CustomCADs.Domain.DataConstants;

namespace CustomCADs.API.Endpoints.Cads.PostCad
{
    using static StatusCodes;

    public class PostCadEndpoint(ICadService service, IWebHostEnvironment env) : Endpoint<PostCadRequest, PostCadResponse>
    {
        public override void Configure()
        {
            Post("");
            Group<CadsGroup>();
            Description(d => d
                .WithSummary("Creates a Cad entity in the database, max file size is 300MB.")
                .Accepts<PostCadRequest>("multipart/form-data")
                .Produces<PostCadResponse>(Status201Created, "application/json"));
        }

        public override async Task HandleAsync(PostCadRequest req, CancellationToken ct)
        {
            CadModel model = req.Adapt<CadModel>();
            model.CreatorId = User.GetId();
            model.CreationDate = DateTime.Now;
            model.Status = User.IsInRole(RoleConstants.Designer) ? CadStatus.Validated : CadStatus.Unchecked;

            int id = await service.CreateAsync(model).ConfigureAwait(false);
            model = await service.GetByIdAsync(id).ConfigureAwait(false);

            string imagePath = await env.UploadImageAsync(req.Image, model.Name + id + req.Image.GetFileExtension()).ConfigureAwait(false);
            string cadPath = await env.UploadCadAsync(req.File, model.Name + id, req.File.GetFileExtension()).ConfigureAwait(false);
            await service.SetPathsAsync(id, cadPath, imagePath).ConfigureAwait(false);

            CadModel createdModel = await service.GetByIdAsync(id).ConfigureAwait(false);
            PostCadResponse response = createdModel.Adapt<PostCadResponse>();

            await SendCreatedAtAsync<GetCadEndpoint>(new { id }, response).ConfigureAwait(false);
        }
    }
}
