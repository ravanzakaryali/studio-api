using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Space.Application.Extensions;

namespace Space.Application.Handlers;

public record UserLoginQuery : IRequest<GetUserResponseDto> { }
internal class UserLoginQueryHandler : IRequestHandler<UserLoginQuery, GetUserResponseDto>
{

    readonly IHttpContextAccessor _contextAccessor;
    readonly UserManager<User> _userManager;

    public UserLoginQueryHandler(IHttpContextAccessor contextAccessor, UserManager<User> userManager, IMapper mapper)
    {
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<GetUserResponseDto> Handle(UserLoginQuery request, CancellationToken cancellationToken)
    {
        string userId = _contextAccessor.HttpContext?.User?.GetLoginUserId()
           ?? throw new UnauthorizedAccessException();

        User? user = await _userManager.Users
            .Where(c => c.Id ==  int.Parse(userId))
            .Include(c => c.UserRoles).ThenInclude(c => c.Role)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new UnauthorizedAccessException();
        return new GetUserResponseDto()
        {
            Id = int.Parse(userId),
            Surname = user.Surname,
            Name = user.Name,
            RoleName = user.UserRoles.Select(u => u.Role.Name).ToList()
        };
    }
}
