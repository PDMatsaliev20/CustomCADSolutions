using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Users.GetUser
{
    using static StatusCodes;

    public class GetUserEndpoint(IUserService service) : Endpoint<GetUserRequest, UserResponseDto>
    {
        public override void Configure()
        {
            Get("{username}");
            Group<UsersGroup>();
            Description(d => d
                .WithSummary("Gets a User by the specified name.")
                .Produces<UserResponseDto>(Status200OK, "application/json")
                .ProducesProblem(Status404NotFound));
        }

        public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
        {
            UserModel model = await service.GetByNameAsync(req.Username);

            UserResponseDto response = new()
            {
                Email = model.Email,
                Username = model.UserName,
                Role = model.RoleName,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
