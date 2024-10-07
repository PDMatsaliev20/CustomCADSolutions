using CustomCADs.Application.Contracts;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CustomCADs.Infrastructure.Email
{
    public class MailKitService(IOptions<EmailOptions> options) : IEmailService
    {
        private const string Server = "smtp.gmail.com";
        private const string From = "customcads414@gmail.com";
        private const SecureSocketOptions Options = SecureSocketOptions.StartTls;
        private readonly int port = options.Value.Port;
        private readonly string password = options.Value.Password;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                MailboxAddress toEmail = new("", to), fromEmail = new("", From);

                MimeMessage message = new()
                {
                    Subject = subject,
                    Body = new TextPart("plain") { Text = body },
                };
                message.From.Add(fromEmail);
                message.To.Add(toEmail);

                using SmtpClient client = new();
                await client.SendMessageAsync(Server, port, Options, From, password, message).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SendVerificationEmailAsync(string to, string endpoint)
        {
            try
            {
                MailboxAddress toEmail = new("", to), fromEmail = new("", From);
                string html = @$"
<h2>Welcome to CustomCADs!</h2>
<p>Please confirm your email by clicking the button below:</p>
<a href='{endpoint}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px;'>Confirm Email</a>
";
                MimeMessage message = new()
                {
                    Subject = "Confirm your email at CustomCADs",
                    Body = new BodyBuilder() { HtmlBody = html }.ToMessageBody()
                };
                message.From.Add(fromEmail);
                message.To.Add(toEmail);

                using SmtpClient client = new();
                await client.SendMessageAsync(Server, port, Options, From, password, message).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendForgotPasswordEmailAsync(string to, string endpoint)
        {
            try
            {
                MailboxAddress toEmail = new("", to), fromEmail = new("", From);

                string html = @$"
<h2>We've got you!</h2>
<h6>Click this button to set a new password.</h6>
<a href='{endpoint}' style='background-color: #4CAF50; color: white; padding: 10px 20px; text-align: center; text-decoration: none; display: inline-block; border-radius: 5px;'>Reset Password</a>
";

                MimeMessage message = new()
                {
                    Subject = "Forgot your CustomCADs password?",
                    Body = new BodyBuilder() { HtmlBody = html }.ToMessageBody()
                };
                message.From.Add(fromEmail);
                message.To.Add(toEmail);

                using SmtpClient client = new();
                await client.SendMessageAsync(Server, port, Options, From, password, message).ConfigureAwait(false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
