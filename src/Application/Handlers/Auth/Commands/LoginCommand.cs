namespace Space.Application.Handlers;

public record LoginCommand : IRequest
{
   
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
internal class LoginCommandHandler : IRequestHandler<LoginCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<User> _userManager;

    public LoginCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //if (!await _unitOfWork.IdentityService.RecaptchaVerifyAsync(request.ReCaptchaToken))
        //{
        //    throw new UnauthorizedAccessException();
        //}
        User user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken: cancellationToken) ??
             throw new NotFoundException(nameof(User), request.Email);


        LoginResponseDto response = await _unitOfWork.IdentityService.LoginAsync(user, request.Password);
        Token token = _unitOfWork.TokenService.GenerateToken(response.User, TimeSpan.FromSeconds(10), response.Roles);
        string refreshToken = _unitOfWork.TokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = token.Expires.AddMinutes(15);

        _contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            Expires = token.Expires.AddDays(7),
            HttpOnly = false,
            SameSite = SameSiteMode.None,
            Secure = true,
        });
        await _userManager.UpdateAsync(user);
    }
}
