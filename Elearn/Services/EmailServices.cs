using System;
using Elearn.Helpers;
using Elearn.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace Elearn.Services
{
	public class EmailServices: IEmailService
	{
        private readonly IConfiguration _config;
        private readonly EmailSettings _emailSettings;

        public EmailServices(IConfiguration config)
        {
            _config = config;
        }



        public void Send(string to, string subject, string html, string from = null)
        {
            // create email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _emailSettings.FromAddress));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            // send email
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSettings.Server, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.Username, _emailSettings.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}