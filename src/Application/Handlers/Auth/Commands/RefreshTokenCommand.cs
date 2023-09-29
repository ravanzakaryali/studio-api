using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Serilog;
using System.Security.Claims;

namespace Space.Application.Handlers;

public class RefreshTokenCommand : IRequest<AccessTokenResponseDto>
{
    public string RefreshToken { get; set; } = null!;
    //public string AccessToken { get; set; } = null!;
}


internal class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AccessTokenResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<User> _userManager;
    readonly ICurrentUserService _currentUserService;


    public RefreshTokenCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor, UserManager<User> userManager, ICurrentUserService currentUserService)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
        _currentUserService = currentUserService;
    }

    public async Task<AccessTokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        if (_contextAccessor.HttpContext.Request.Cookies.TryGetValue("token", out string? token2))
        {
            Console.WriteLine($"\n\n\nToken2: {token2}\n\n\n");
        }
        if (_currentUserService.UserId == null) throw new UnauthorizedAccessException();
        //ClaimsPrincipal principal = _unitOfWork.TokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        //if (principal.Identity is null) throw new TokenExpiredException();
        //string? username = principal.Identity.Name ?? throw new TokenExpiredException();

        User? user = await _unitOfWork.UserService.FindById(new Guid(_currentUserService.UserId));

        if (user == null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpires <= DateTime.UtcNow)
            throw new TokenExpiredException();

        IList<string> roles = await _unitOfWork.RoleService.GetRolesByUser(user);

        Token token = _unitOfWork.TokenService.GenerateToken(user, TimeSpan.FromMinutes(45), roles);

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
        //await _userManager.UpdateAsync(user);
        return new AccessTokenResponseDto()
        {
            //UserId = user.Id,
            RefreshToken = refreshToken,
            Token = token.AccessToken,
            //TokenExpires = token.Expires,
        };
    }
}
