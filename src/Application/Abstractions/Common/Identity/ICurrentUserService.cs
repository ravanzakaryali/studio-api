namespace Space.Application.Abstractions;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? Username { get; }
    string? Email { get; }
}
