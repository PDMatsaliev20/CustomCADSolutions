using FastEndpoints;

namespace CustomCADs.API.Endpoints.Users.GetUser;

public class GetUserRequest
{
    [BindFrom("username")]
    public required string Username { get; set; }
}
