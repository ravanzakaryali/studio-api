using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Space.Domain.Entities;
using System.Security.Claims;
namespace Space.Application.Handlers;

public record LoginCommand : IRequest<AccessTokenResponseDto>
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    //public string ReCaptchaToken { get; set; } = null!;
}
internal class LoginCommandHandler : IRequestHandler<LoginCommand, AccessTokenResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IHttpContextAccessor _contextAccessor;



    public LoginCommandHandler(IUnitOfWork unitOfWork, IHttpContextAccessor contextAccessor)
    {
        _unitOfWork = unitOfWork;
        _contextAccessor = contextAccessor;
    }

    public async Task<AccessTokenResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        //if (!await _unitOfWork.IdentityService.RecaptchaVerifyAsync(request.ReCaptchaToken))
        //{
        //    throw new UnauthorizedAccessException();
        //}
        LoginResponseDto response = await _unitOfWork.IdentityService.LoginAsync(request.Email, request.Password);
        Token token = _unitOfWork.TokenService.GenerateToken(response.User, response.Roles);
        string refreshToken = _unitOfWork.TokenService.GenerateRefreshToken();
        response.User.RefreshToken = refreshToken;
        response.User.RefreshTokenExpires = token.Expires.AddMinutes(15);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            Expires = token.Expires,
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true,
        });
        return new AccessTokenResponseDto()
        {
            //UserId = response.User.Id,
            //RefreshToken = refreshToken,
            //Token = token.AccessToken,
            //TokenExpires = token.Expires,
        };
    }
}
