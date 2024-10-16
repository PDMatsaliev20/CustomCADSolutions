using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Roles.GetRole;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Roles;
using CustomCADs.Application.UseCases.Roles.Commands.Create;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Roles.PostRole;

using static StatusCodes;

public class PostRoleEndpoint(IMediator mediator, IAppRoleManager manager) : Endpoint<PostRoleRequest, RoleResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<RolesGroup>();
        Description(d => d
            .WithSummary("Creates a Role with the specified name.")
            .Produces<RoleResponseDto>(Status201Created, "application/json")
            .ProducesProblem(Status400BadRequest));
    }

    public override async Task HandleAsync(PostRoleRequest req, CancellationToken ct)
    {
        IdentityResult result = await manager.CreateAsync(new(req.Name)).ConfigureAwait(false);
        if (!result.Succeeded)
        {
            var failures = result.Errors.Select(e => new ValidationFailure()
            {
                ErrorMessage = e.Description
            });
            ValidationFailures.AddRange(failures);

            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        RoleModel model = new()
        {
            Name = req.Name,
            Description = req.Description,
        };
        CreateRoleCommand command = new(model);
        await mediator.Send(command).ConfigureAwait(false);

        RoleResponseDto response = new()
        {
            Name = model.Name,
            Description = model.Description,
        };
        await SendCreatedAtAsync<GetRoleEndpoint>(new { model.Name }, response).ConfigureAwait(false);
    }
}
