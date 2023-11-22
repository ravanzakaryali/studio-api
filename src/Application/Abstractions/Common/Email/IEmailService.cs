namespace Space.Application.Abstractions;

public interface IEmailService
{
    Task SendMessageAsync(string Message, string email, string emailTemplatepath = "EmailTemplate.html", string subject = "Confirm Code");
}
