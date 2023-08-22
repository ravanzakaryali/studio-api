namespace Space.Application.Exceptions.Authetication;

public class UnauthorizedException : AutheticationException
{
    public UnauthorizedException() : base("Unauthorized: Access Denied")
    {
    }
}
