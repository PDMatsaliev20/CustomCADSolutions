using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.ForgotPassword
{
    public class ForgotPasswordRequest
    {
        [BindFrom("email")]
        public required string Email { get; set; }
    }
}
