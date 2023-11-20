namespace Space.Application.Handlers;

public class ConfirmCodeCommand : IRequest<AccessTokenResponseDto>
{
    public string Code { get; set; } = null!;
    public string Email { get; set; } = null!;
}
internal class UserConfirmCodeCommandHandler : IRequestHandler<ConfirmCodeCommand, AccessTokenResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly ISpaceDbContext _spaceDbContext;

    public UserConfirmCodeCommandHandler(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor contextAccessor,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<AccessTokenResponseDto> Handle(ConfirmCodeCommand request, CancellationToken cancellationToken)
    {
        User user = await _unitOfWork.UserService.FindByEmailAsync(request.Email);
        if (user.ConfirmCode == request.Code &&
            user.ConfirmCodeExpires < DateTime.UtcNow)
            throw new TokenExpiredException();
        user.ConfirmCode = null;
        Token token = _unitOfWork.TokenService.GenerateToken(user, TimeSpan.FromSeconds(15));
        string refreshToken = _unitOfWork.TokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = token.Expires.AddMinutes(15);
        await _spaceDbContext.SaveChangesAsync();
        _contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            Expires = token.Expires,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
        });
        return new AccessTokenResponseDto()
        {
            //RefreshToken = refreshToken,
            //Token = token.AccessToken,
            //TokenExpires = token.Expires,
            //UserId = user.Id
        };
    }
}
