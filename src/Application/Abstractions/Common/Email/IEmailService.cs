namespace Space.Application.Abstractions;

public interface IEmailService
{
    Task SendMessageAsync(string Message, string email, string subject = "Confirm Code");
}
