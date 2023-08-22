namespace Space.Application.Exceptions;

public class AlreadyExistsException : ApplicationException
{

    public HttpStatusCode HttpStatusCode = HttpStatusCode.Conflict;
    public AlreadyExistsException() : base("The resource already exists.") { }
    public AlreadyExistsException(string? message) : base(message) { }
    public AlreadyExistsException(string name, object key)
       : base($"This '{key}' is already in use. Please choose another name for {name}.")
    {
    }
    public AlreadyExistsException(string? message, Exception? innerException) : base(message, innerException) { }
}
