using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace CustomCADs.Infrastructure.Email
{
    public static class Utilities
    {
        public static async Task SendMessageAsync(this SmtpClient client, string server, int port, SecureSocketOptions options, string from, string password, MimeMessage message)
        {
            await client.ConnectAsync(server, port, options).ConfigureAwait(false);
            await client.AuthenticateAsync(from, password).ConfigureAwait(false);
            await client.SendAsync(message).ConfigureAwait(false);
            await client.DisconnectAsync(quit: true).ConfigureAwait(false);
        }
    }
}
