namespace Space.Application.Abstractions;

public interface IEmailService
{
    Task SendMessageAsync(string Message, string email, string emailTemplatepath, string subject = "Confirm Code");
}
