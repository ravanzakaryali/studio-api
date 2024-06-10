using System.Reflection.Metadata.Ecma335;

namespace Space.Application.Handlers.Queries;

public class GetAllNotificationsQuery : IRequest<IEnumerable<GetNotificationDto>>
{

}
internal class GetAllNotificationsQueryHandler : IRequestHandler<GetAllNotificationsQuery, IEnumerable<GetNotificationDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllNotificationsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetNotificationDto>> Handle(GetAllNotificationsQuery request, CancellationToken cancellationToken)
    {
        List<Notification> notifications = await _spaceDbContext.Notifications
            .Include(n => n.FromUser)
            .ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<GetNotificationDto> response = notifications.Select(n =>
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
                Title = n.Title,
                Content = n.Content,
                IsRead = n.IsRead,
                FromUser = user
            };
        });



        return response;
    }
}