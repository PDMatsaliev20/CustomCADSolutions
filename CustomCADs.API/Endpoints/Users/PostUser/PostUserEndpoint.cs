using CustomCADs.API.Dtos;
using CustomCADs.API.Endpoints.Users.GetUser;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Infrastructure.Identity;
using CustomCADs.Infrastructure.Identity.Contracts;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace CustomCADs.API.Endpoints.Users.PostUser
{
    using static ApiMessages;
    using static StatusCodes;

    public class PostUserEndpoint(IAppUserManager manager, IUserService service, IRoleService roleService) : Endpoint<PostUserRequest, UserResponseDto>
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
            bool roleExists = await roleService.ExistsByNameAsync(req.Role).ConfigureAwait(false);
            if (!roleExists)
            {
                string roles = string.Join(", ", roleService.GetAllNames());
                string message = string.Format(InvalidRole, roles);
                await SendResultAsync(Results.BadRequest(message)).ConfigureAwait(false);
                return;
            }

            AppUser user = new(req.Username, req.Email);
            IdentityResult result = await manager.CreateAsync(user).ConfigureAwait(false);
            if (!result.Succeeded)
            {
                await SendResultAsync(Results.BadRequest(result.Errors)).ConfigureAwait(false);
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
            string id = await service.CreateAsync(model);

            UserModel addedUser = await service.GetByIdAsync(id).ConfigureAwait(false);

            UserResponseDto response = new()
            {
                Email = addedUser.Email,
                Username = addedUser.UserName,
                Role = addedUser.RoleName,
                FirstName = addedUser.FirstName,
                LastName = addedUser.LastName,
            };
            await SendCreatedAtAsync<GetUserEndpoint>(req.Username, response).ConfigureAwait(false);
        }
    }
}
