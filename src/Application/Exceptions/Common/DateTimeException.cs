namespace Space.Application.Exceptions;

public class DateTimeException : ApplicationException
{
    public HttpStatusCode HttpStatusCode = HttpStatusCode.Conflict;
    public TimeSpan Time { get; set; }
    public DateTimeException() : base("The requested resource was not found.") { }
    public DateTimeException(string? message) : base(message) { }
    public DateTimeException(string name, TimeSpan date)
       : base(name)
    {
        Time = date;
    }
    public DateTimeException(string? message, Exception? innerException) : base(message, innerException) { }
}
