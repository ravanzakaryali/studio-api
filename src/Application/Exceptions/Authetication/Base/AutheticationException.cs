namespace Space.Application.Exceptions;

public class AutheticationException : ApplicationException
{
    public const HttpStatusCode StatusCode = HttpStatusCode.Unauthorized;
    public AutheticationException() : base("Authetication exception") { }

    public AutheticationException(string? message) : base(message) { }

    public AutheticationException(string? message, Exception? innerException) : base(message, innerException) { }
}
