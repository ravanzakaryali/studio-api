using System.Security.Claims;

namespace Space.Application.Handlers;

public class RefreshTokenCommand : IRequest<AccessTokenResponseDto>
{
    public string RefreshToken { get; set; } = null!;
    public string AccessToken { get; set; } = null!;
}


internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessTokenResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;

    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task<AccessTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        ClaimsPrincipal principal = _unitOfWork.TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        if (principal.Identity is null) throw new TokenExpiredException();
        string? username = principal.Identity.Name ?? throw new TokenExpiredException();

        User? user = await _unitOfWork.UserService.FindByNameAsync(username);
        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpires <= DateTime.UtcNow)
            throw new TokenExpiredException();

        IList<string> roles = await _unitOfWork.RoleService.GetRolesByUser(user);

        Token token = _unitOfWork.TokenService.GenerateToken(user, roles);

        string refreshToken = _unitOfWork.TokenService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpires = token.Expires.AddMinutes(15);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            Expires = token.Expires,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });
        return new AccessTokenResponseDto()
        {
            //UserId = user.Id,
            //RefreshToken = refreshToken,
            //Token = token.AccessToken,
            //TokenExpires = token.Expires,
        };
    }
}
