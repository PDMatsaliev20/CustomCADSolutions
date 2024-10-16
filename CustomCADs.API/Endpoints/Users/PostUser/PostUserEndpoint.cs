using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Users.GetUser;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.UseCases.Roles.Queries.ExistsByName;
using CustomCADs.Application.UseCases.Roles.Queries.GetAllNames;
using CustomCADs.Application.UseCases.Users.Commands.Create;
using CustomCADs.Application.UseCases.Users.Queries.GetById;
using CustomCADs.Auth;
using CustomCADs.Auth.Contracts;
using FastEndpoints;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Users.PostUser;

using static ApiMessages;
using static StatusCodes;

public class PostUserEndpoint(IMediator mediator, IAppUserManager manager) : Endpoint<PostUserRequest, UserResponseDto>
{
    public override void Configure()
    {
        Post("");
        Group<UsersGroup>();
        Description(d => d
            .WithSummary("Creates a User with the specified name and role.")
            .Produces<UserResponseDto>(Status201Created, "application/json")
            .ProducesProblem(Status400BadRequest));
    }

    public override async Task HandleAsync(PostUserRequest req, CancellationToken ct)
    {
        RoleExistsByNameQuery existsByNameQuery = new(req.Role);
        bool roleExists = await mediator.Send(existsByNameQuery).ConfigureAwait(false);

        if (!roleExists)
        {
            GetAllRoleNamesQuery getRoleNamesQuery = new();
            var roleNames = await mediator.Send(getRoleNamesQuery).ConfigureAwait(false);

            ValidationFailures.Add(new()
            {
                ErrorMessage = string.Format(InvalidRole, string.Join(", ", roleNames))
            });
            await SendErrorsAsync().ConfigureAwait(false);
            return;
        }

        AppUser user = new(req.Username, req.Email);
        IdentityResult result = await manager.CreateAsync(user).ConfigureAwait(false);
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
        await manager.AddToRoleAsync(user, req.Role).ConfigureAwait(false);
        
        UserModel model = new()
        {
            Email = req.Email,
            UserName = req.Username,
            RoleName = req.Role,
            FirstName = req.FirstName,
            LastName = req.LastName,
        };
        CreateUserCommand command = new(model);
        string id = await mediator.Send(command).ConfigureAwait(false);

        GetUserByIdQuery query = new(id);
        UserModel addedUser = await mediator.Send(query).ConfigureAwait(false);

        UserResponseDto response = new()
        {
            Email = addedUser.Email,
            Username = addedUser.UserName,
            Role = addedUser.RoleName,
            FirstName = addedUser.FirstName,
            LastName = addedUser.LastName,
        };
        await SendCreatedAtAsync<GetUserEndpoint>(new { req.Username }, response).ConfigureAwait(false);
    }
}
