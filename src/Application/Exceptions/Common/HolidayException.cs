namespace Space.Application.Exceptions;

public class HolidayException : ApplicationException
{

    public HttpStatusCode HttpStatusCode = HttpStatusCode.Conflict;
    public HolidayException() : base(
        "HolidayException: The requested resource was not found."
    )
    { }
    public HolidayException(string? message) : base(message) { }
    public HolidayException(string? message, Exception? innerException) : base(message, innerException) { }
}
