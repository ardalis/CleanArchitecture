﻿using Clean.Architecture.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Clean.Architecture.Infrastructure.Email;

public class FakeEmailSender(ILogger<FakeEmailSender> logger) : IEmailSender
{
  public Task SendEmailAsync(string to, string from, string subject, string body)
  {
    logger.LogInformation("Not actually sending an email to {to} from {from} with subject {subject}", to, from, subject);
    return Task.CompletedTask;
  }
}
