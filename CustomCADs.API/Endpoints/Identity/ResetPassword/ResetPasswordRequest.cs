using FastEndpoints;

namespace CustomCADs.API.Endpoints.Identity.ResetPassword
{
    public class ResetPasswordRequest
    {
        [BindFrom("email")]
        public required string Email { get; set; }
        public string? Token { get; set; }
        public string? NewPassword { get; set; }

    }
}
