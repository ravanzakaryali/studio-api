namespace Space.Application.Abstractions;

public interface IEmailService
{
    Task SendMessageAsync(string message, string email, string emailTemplatepath = "EmailTemplate.html", string subject = "Confirm Code");
    Task SendMessageAsync(string message, string className, string name, string email, string emailTemplatepath = "EmailTemplate.html", string subject = "Confirm Code");

    Task SendSupportMessageAsync(SendEmailSupportMessageDto sendEmailSupportMessage, int supportId);



}
