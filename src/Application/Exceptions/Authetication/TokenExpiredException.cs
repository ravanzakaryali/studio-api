namespace Space.Application.Exceptions;

public class TokenExpiredException : AutheticationException
{
    public TokenExpiredException() : base("Access token has expired. Please obtain a new access token to continue accessing the resource.") { }

    public TokenExpiredException(string? message) : base(message)
    {
    }
}
