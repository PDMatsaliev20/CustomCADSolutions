using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.ResetPassword
{
    public class ResetPasswordRequest
    {
        [BindFrom("email")]
        public required string Email { get; set; }

        [QueryParam]
        public required string Token { get; set; }

        [QueryParam]
        public required string NewPassword { get; set; }

    }
}
