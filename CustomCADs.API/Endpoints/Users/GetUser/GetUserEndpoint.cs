using AutoMapper;
using CustomCADs.API.Models.Users;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using CustomCADs.Application.Services;
using FastEndpoints;

namespace CustomCADs.API.Endpoints.Users.GetUser
{
    using static StatusCodes;

    public class GetUserEndpoint(IUserService service) : Endpoint<GetUserRequest, UserGetDTO>
    {
        public override void Configure()
        {
            Get("{username}");
            Group<UsersGroup>();
            Description(d => d.WithSummary("Gets a User by the specified name."));
            Options(opt =>
            {
                opt.Produces<UserGetDTO>(Status200OK, "application/json");
                opt.ProducesProblem(Status404NotFound);
            });
        }

        public override async Task HandleAsync(GetUserRequest req, CancellationToken ct)
        {
            UserModel model = await service.GetByNameAsync(req.Username);

            UserGetDTO response = new()
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
