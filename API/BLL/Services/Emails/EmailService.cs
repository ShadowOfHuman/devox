using API.BLL.Helpers;
using API.BLL.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BLL.Services.Emails
{
    class EmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Служба поддержи DevOX", email.SenderEmail));
            emailMessage.To.Add(new MailboxAddress(email.RecipientEmail));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = email.Content
            };
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.None, cancellationToken);

                //No need to implement authentication yet
                //client.AuthenticationMechanisms.Remove("XOAUTH2");

                await client.SendAsync(emailMessage, cancellationToken);
                await client.DisconnectAsync(true, cancellationToken);
            }
        }
    }
}
