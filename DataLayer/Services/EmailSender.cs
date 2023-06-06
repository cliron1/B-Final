using System.Net.Mail;
using System.Net;
using BezeqFinalProject.Common.Models;
using BezeqFinalProject.Common.Models.Settings;
using Microsoft.Extensions.Configuration;

namespace BezeqFinalProject.Common.Services;

public interface IEmailSender {
    Task Send(string to, string subject, string body);
    Task Send(EmailMsgmodel model);
}

public class EmailSender : IEmailSender {
    private readonly EmailSettings settings;

    public EmailSender(IConfiguration config) {
        settings = config.GetSection("AppConfig:Email").Get<EmailSettings>();
    }

    public Task Send(string to, string subject, string body)
        => Send(new EmailMsgmodel { To = to, Subject = subject, Body = body });

    public async Task Send(EmailMsgmodel model) {
        using var smtpClient = new SmtpClient(settings.Smtp, settings.Port);
        smtpClient.UseDefaultCredentials = false;
        smtpClient.Credentials = new NetworkCredential(settings.User, settings.Pwd);

        using var message = new MailMessage();
        message.From = new MailAddress(
            settings.From.Address,
            settings.From.DisplayName
        );
        message.To.Add(model.To);
        if(!string.IsNullOrEmpty(model.Cc))
            message.CC.Add(model.Cc);
        if(!string.IsNullOrEmpty(model.Bcc))
            message.Bcc.Add(model.Bcc);
        message.Subject = model.Subject;
        message.IsBodyHtml = model.IsBodyHtml;
        message.Body = model.Body;

        try {
            await smtpClient.SendMailAsync(message);

        } catch(Exception ex) {
            throw new Exception("Error is sending mail", ex);
        }
    }
}
