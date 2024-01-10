namespace Space.Application.Exceptions;

public class InvalidCredentialsException : AutheticationException
{
    public InvalidCredentialsException() : base("Invalid credentials. Please check your email and password and try again") { }

    public InvalidCredentialsException(string? message) : base(message)
    {
    }
}
