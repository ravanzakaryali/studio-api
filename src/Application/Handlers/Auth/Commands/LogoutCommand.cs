namespace Space.Application.Handlers;

public record LogoutCommand : IRequest { }

internal class LogoutCommandHandler : IRequestHandler<LogoutCommand>
{
    readonly IHttpContextAccessor _contextAccessor;
    readonly ICurrentUserService _currentUserService;
    readonly UserManager<User> _userManager;

    public LogoutCommandHandler(
        IHttpContextAccessor contextAccessor,
        ICurrentUserService currentUserService,
        UserManager<User> userManager)
    {
        _contextAccessor = contextAccessor;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        string? userId = _currentUserService.UserId
            ?? throw new Exception("User not login");
        _ = await _userManager.FindByIdAsync(userId.ToString())
            ?? throw new UnauthorizedAccessException();
        _contextAccessor.HttpContext?.Response.Cookies.Delete("token", new CookieOptions()
        {
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
        });
    }
}
