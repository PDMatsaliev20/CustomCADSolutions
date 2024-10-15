using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.EmailConfirmed;

public class EmailConfirmedRequest
{
    [BindFrom("username")]
    public required string Username { get; set; }
}
