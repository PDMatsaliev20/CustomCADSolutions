namespace CustomCADs.Application.Common.Contracts;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendVerificationEmailAsync(string to, string endpoint);
    Task SendForgotPasswordEmailAsync(string to, string endpoint);
}
