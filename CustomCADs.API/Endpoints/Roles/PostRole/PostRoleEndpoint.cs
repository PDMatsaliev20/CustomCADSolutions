using AutoMapper;
using CustomCADs.API.Endpoints.Roles.GetRole;
using CustomCADs.API.Models.Roles;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.Services;
using CustomCADs.Infrastructure.Identity.Contracts;
using CustomCADs.Infrastructure.Identity.Managers;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace CustomCADs.API.Endpoints.Roles.PostRole
{
    using static StatusCodes;

    public class PostRoleEndpoint(IAppRoleManager manager, IRoleService service) : Endpoint<PostRoleRequest, RoleGetDTO>
    {
        public override void Configure()
        {
            Post("");
            Group<RolesGroup>();
            Description(d => d.WithSummary("Creates a Role with the specified name."));
            Options(opt =>
            {
                opt.Produces<RoleGetDTO>(Status201Created, "application/json");
                opt.ProducesProblem(Status400BadRequest);
            });
        }

        public override async Task HandleAsync(PostRoleRequest req, CancellationToken ct)
        {
            IdentityResult result = await manager.CreateAsync(new(req.Name)).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                await SendResultAsync(Results.BadRequest(result.Errors)).ConfigureAwait(false);
                return;
            }

            RoleModel model = new()
            {
                Name = req.Name,
                Description = req.Description,
            };
            await service.CreateAsync(model).ConfigureAwait(false);

            RoleGetDTO response = new()
            {
                Name = model.Name,
                Description = model.Description,
            };
            await SendCreatedAtAsync<GetRoleEndpoint>(model.Name, response).ConfigureAwait(false);
        }
    }
}
