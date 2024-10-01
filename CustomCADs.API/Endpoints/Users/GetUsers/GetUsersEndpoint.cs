using CustomCADs.API.Dtos;
using CustomCADs.Application.Contracts;
using CustomCADs.Application.Models.Users;
using FastEndpoints;
using Mapster;

namespace CustomCADs.API.Endpoints.Users.GetUsers
{
    using static StatusCodes;

    public class GetUsersEndpoint(IUserService service) : Endpoint<GetUsersRequest, GetUsersResponse>
    {
        public override void Configure()
        {
            Get("");
            Group<UsersGroup>();
            Description(d => d
                .WithSummary("Gets All Users.")
                .Produces<GetUsersResponse>(Status200OK, "application/json")
                .ProducesProblem(Status400BadRequest)
                .ProducesProblem(Status404NotFound));
        }

        public override async Task HandleAsync(GetUsersRequest req, CancellationToken ct)
        {
            UserResult result = service.GetAll(
                username: req.Name,
                sorting: req.Sorting ?? "",
                page: req.Page,
                limit: req.Limit
            );
            
            GetUsersResponse response = new()
            {
                Count = result.Count,
                Users = result.Users.Select(user => user.Adapt<UserResponseDto>()).ToArray()
            };
            await SendAsync(response, Status200OK).ConfigureAwait(false);
        }
    }
}
