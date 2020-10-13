﻿using System.Net.Mail;
using System.Threading.Tasks;
using Clean.Architecture.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Infrastructure
{
    public class EmailSender : IEmailSender
    {
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger = logger;
        }
        
        public async Task SendEmailAsync(string to, string from, string subject, string body)
        {
            var emailClient = new SmtpClient("localhost");
            var message = new MailMessage
            {

                From = new MailAddress(from),
                Subject = subject,
                Body = body


            };
            message.To.Add(new MailAddress(to));
            await emailClient.SendMailAsync(message);
            _logger.LogWarning($"Sending email to {to} from {from} with subject {subject}.");
        }
    }
}
