namespace Space.Application.Handlers;

public class CreateNotificationCommand : IRequest
{
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
}

internal class CreateNotificationCommandHandler : IRequestHandler<CreateNotificationCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;
    readonly UserManager<User> _userManager;

    public CreateNotificationCommandHandler(
        ISpaceDbContext spaceDbContext, ICurrentUserService currentUserService, UserManager<User> userManager)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
        _userManager = userManager;
    }

    public async Task Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {

        string? loginUserId = _currentUserService.UserId
            ?? throw new UnauthorizedAccessException();

        User user = await _userManager.FindByIdAsync(loginUserId);
        List<User> users = await _userManager.Users.ToListAsync(cancellationToken);

        foreach (User userItem in users)
        {
            Notification notification = new()
            {
                Title = request.Title,
                Content = request.Content,
                FromUser = user,
                FromUserId = user.Id,
                ToUser = userItem,
                ToUserId = userItem.Id,
            };
            _spaceDbContext.Notifications.Add(notification);
        }
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
