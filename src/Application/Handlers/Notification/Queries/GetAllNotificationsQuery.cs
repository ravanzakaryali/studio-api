using System.Reflection.Metadata.Ecma335;

namespace Space.Application.Handlers.Queries;

public class GetAllNotificationsQuery : IRequest<IEnumerable<GetNotificationDto>>
{

}
internal class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IEnumerable<GetNotificationDto>>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;
    readonly UserManager<User> _userManager;

    public GetAllNotificationsQueryHandler(
        ISpaceDbContext spaceDbContext,
        ICurrentUserService currentUserService,
        UserManager<User> userManager)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task<IEnumerable<GetNotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        string? loginUserId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        User user = await _userManager.FindByIdAsync(loginUserId);

        List<Notification> notifications = await _spaceDbContext.Notifications
            .Include(n => n.FromUser)
            .ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<GetNotificationDto> response = notifications.Where(c => c.ToUserId == user.Id).Select(n =>
        {

            UserDto? user = null;
            if (n.FromUser != null)
                user = new UserDto()
                {
                    Id = n.FromUser.Id,
                    Name = n.FromUser.Name,
                    Surname = n.FromUser.Surname,
                    Email = n.FromUser.Email,
                };

            return new GetNotificationDto()
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                IsRead = n.IsRead,
                FromUser = user
            };
        });



        return response;
    }
}