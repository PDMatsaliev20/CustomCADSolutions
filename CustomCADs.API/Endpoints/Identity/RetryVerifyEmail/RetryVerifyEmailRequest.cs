using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.RetryVerifyEmail;

public class RetryVerifyEmailRequest
{
    [BindFrom("username")]
    public required string Username { get; set; }   
}
