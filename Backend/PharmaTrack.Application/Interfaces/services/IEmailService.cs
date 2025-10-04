using System;

namespace PharmaTrack.Application.Interfaces.services;

public interface IEmailService
{
        Task SendEmailAsync(string to, string subject, string body);
}
