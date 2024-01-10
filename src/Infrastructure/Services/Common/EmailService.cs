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
}
