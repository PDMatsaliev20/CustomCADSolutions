﻿using AutoMapper;
using CustomCADs.API.Endpoints.Roles.Responses;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.Services;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Roles.GetRole
{
    using static StatusCodes;

    public class GetRoleEndpoint(IRoleService service) : Endpoint<GetRoleRequest, RoleResponseDto>
    {
        public override void Configure()
        {
            Get("{name}");
            Group<RolesGroup>();
            Description(d => d.WithSummary("Gets a Role by the specified name."));
            Options(opt =>
            {
                opt.Produces<RoleResponseDto>(Status200OK, "application/json");
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(GetRoleRequest req, CancellationToken ct)
        {
            RoleModel role = await service.GetByNameAsync(req.Name).ConfigureAwait(false);

            RoleResponseDto response = new()
            {
                Name = role.Name,
                Description = role.Description,
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
