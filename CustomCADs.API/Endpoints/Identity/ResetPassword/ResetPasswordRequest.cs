namespace CustomCADs.API.Endpoints.Identity.ResetPassword;

public class ResetPasswordRequest
{
    public required string Email { get; set; }
    public required string Token { get; set; }
    public required string NewPassword { get; set; }

}
