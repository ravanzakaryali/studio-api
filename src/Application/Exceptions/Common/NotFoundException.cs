namespace Space.Application.Exceptions;

public class NotFoundException : ApplicationException
{

    public HttpStatusCode HttpStatusCode = HttpStatusCode.NotFound;
    public NotFoundException() : base("The requested resource was not found.") { }
    public NotFoundException(string? message) : base(message) { }
    public NotFoundException(string name, object key)
       : base($"Entity '{name}' ({key}) was not found.")
    {
    }
    public NotFoundException(string? message, Exception? innerException) : base(message, innerException) { }
}
