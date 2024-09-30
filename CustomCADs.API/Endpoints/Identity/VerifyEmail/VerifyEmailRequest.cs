using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.VerifyEmail
{
    public class VerifyEmailRequest
    {
        [BindFrom("username")]
        public required string Username { get; set; }
        
        public required string Token { get; set; }
    }
}
