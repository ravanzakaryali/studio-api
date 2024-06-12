
namespace Space.Application.Handlers;


public class UpdateNotificationCommand : IRequest
{
    public int Id { get; set; }
}
internal class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly UserManager<User> _userManager;
    readonly ICurrentUserService _currentUserService;

    public UpdateNotificationCommandHandler(
        ISpaceDbContext spaceDbContext,
        ICurrentUserService currentUserService,
        UserManager<User> userManager)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {

        string? loginUserId = _currentUserService.UserId
           ?? throw new UnauthorizedAccessException();

        User user = await _userManager.FindByIdAsync(loginUserId);

        Notification? notification = await _spaceDbContext.Notifications
           .FirstOrDefaultAsync(n => n.Id == request.Id && n.ToUserId == user.Id, cancellationToken)
                   ?? throw new NotFoundException(nameof(Notification), request.Id);

        notification.IsRead = true;
        _spaceDbContext.Notifications.Update(notification);
        await _spaceDbContext.SaveChangesAsync();
    }
}