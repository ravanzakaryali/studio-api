using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Space.Infrastructure.Services;

public class EmailService : IEmailService
{
    readonly IConfiguration _configuration;
    readonly SmtpClient _smtpClient;
    readonly IWebHostEnvironment _webHostEnvironment;


    public EmailService(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _configuration = configuration;
        _smtpClient = new SmtpClient(configuration["SMTP:Host"], configuration.GetValue<int>("SMTP:Port"))
        {
            Credentials = new NetworkCredential(configuration["SMTP:Email"], configuration["SMTP:Password"]),
            EnableSsl = true
        };
        _webHostEnvironment = webHostEnvironment;
    }



    public async Task SendMessageAsync(string message, string email, string emailTemplate = "EmailTemplate.html", string subject = "Confirm Code")
    {
        string fromMail = _configuration["SMTP:Email"];
        MailMessage mailMessage = new(fromMail, email, subject, message)
        {
            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
        };
        string htmlTemplate = IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, emailTemplate));
        string emailContent = htmlTemplate.Replace("{{link}}", message);
        mailMessage.Body = emailContent;
        await _smtpClient.SendMailAsync(mailMessage);
    }
    public async Task SendMessageAsync(string message, string className, string name, string email, string emailTemplate = "EmailTemplate.html", string subject = "Confirm Code")
    {
        string fromMail = _configuration["SMTP:Email"];
        MailMessage mailMessage = new(fromMail, email, subject, message)
        {
            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
        };
        string htmlTemplate = IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, emailTemplate));
        string emailContent = htmlTemplate.Replace("{{link}}", message).Replace("{{name}}", name).Replace("{{className}}", className);
        mailMessage.Body = emailContent;
        await _smtpClient.SendMailAsync(mailMessage);
    }

    public async Task SendSupportMessageAsync(SendEmailSupportMessageDto sendEmailSupportMessage, int supportId)
    {
        string fromMail = _configuration["SMTP:Email"];
        MailMessage mailMessage = new()
        {
            From = new MailAddress(fromMail),
            Subject = "Studio - Dəstək",
            IsBodyHtml = true,
            BodyEncoding = Encoding.UTF8,
        };

        foreach (string toEmail in sendEmailSupportMessage.To)
        {
            mailMessage.To.Add(toEmail);
        }

        string htmlTemplate = IO.File.ReadAllText(Path.Combine(_webHostEnvironment.WebRootPath, "EmailSupportTemplate.html"));
        string emailContent = htmlTemplate
                                .Replace("{{message}}", sendEmailSupportMessage.Message)
                                .Replace("{{name}}", sendEmailSupportMessage.User.Name)
                                .Replace("{{title}}", sendEmailSupportMessage.Title)
                                .Replace("{{link}}", $"{_configuration["App:ClientUrl"]}/admin/app/supports?supportId={supportId}")
                                .Replace("{{surname}}", sendEmailSupportMessage.User.Surname)
                                .Replace("{{email}}", sendEmailSupportMessage.User.Email)
                                .Replace("{{class}}", sendEmailSupportMessage.Class?.Name ?? "Yoxdur");

        mailMessage.Body = emailContent;
        await _smtpClient.SendMailAsync(mailMessage);
    }
}
