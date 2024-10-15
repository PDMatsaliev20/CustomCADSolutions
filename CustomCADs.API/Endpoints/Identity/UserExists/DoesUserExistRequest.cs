using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.UserExists;

public class UserExistsRequest
{
    [BindFrom("username")]
    public required string Username { get; set; }
}
