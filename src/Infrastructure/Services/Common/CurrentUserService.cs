using Microsoft.AspNetCore.Http;
using Space.Application.Extensions;

namespace Space.Infrastructure.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _claims;

    public CurrentUserService(IHttpContextAccessor claims)
    {
        _claims = claims;
    }
    public string? UserId => _claims.HttpContext?.User.GetLoginUserId();

    public string? Username => _claims.HttpContext?.User.GetLoginUserName();

    public string? Email => _claims.HttpContext?.User.GetLoginUserEmail();
}
