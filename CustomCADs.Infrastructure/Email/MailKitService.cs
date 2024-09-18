﻿using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace CustomCADs.Infrastructure.Email
{
    public class MailKitService(IConfiguration config)
    {
        private readonly string server = "smtp.gmail.com";
        private readonly int port = 587;
        private readonly string password = config["Email:AppPassword"] ?? throw new ArgumentNullException("No App Password provided.");
        private readonly string from = "customcads414@gmail.com";
        private readonly SecureSocketOptions options = SecureSocketOptions.StartTls;

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                MailboxAddress toEmail = new("", to), fromEmail = new("", from);

                MimeMessage message = new()
                {
                    Subject = subject,
                    Body = new TextPart("plain") { Text = body },
                };
                message.From.Add(fromEmail);
                message.To.Add(toEmail);

                using SmtpClient client = new();
                await client.ConnectAsync(server, port, options).ConfigureAwait(false);
                await client.AuthenticateAsync(from, password).ConfigureAwait(false);
                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(quit: true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
