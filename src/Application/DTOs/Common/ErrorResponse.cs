namespace Space.Application.DTOs;

public class ErrorResponse
{
    public string Title { get; set; } = null!;
}

public class ValidationErrorResponse : ErrorResponse
{
    public IDictionary<string, string[]>? Errors { get; set; }
}

public class TimeErrorResponse
{
    public string Message { get; set; } = null!;
    public TimeSpan Time { get; set; }
}