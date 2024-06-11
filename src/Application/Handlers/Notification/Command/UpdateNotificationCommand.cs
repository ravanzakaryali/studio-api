
namespace Space.Application.Handlers;


public class UpdateNotificationCommand : IRequest
{
    public int Id { get; set; }
}
internal class UpdateNotificationCommandHandler : IRequestHandler<UpdateNotificationCommand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public UpdateNotificationCommandHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        Notification? notification = await _spaceDbContext.Notifications
           .FirstOrDefaultAsync(n => n.Id == request.Id, cancellationToken)
                   ?? throw new NotFoundException(nameof(Notification), request.Id);


        notification.IsRead = true;
        _spaceDbContext.Notifications.Update(notification);
        await _spaceDbContext.SaveChangesAsync();
    }
}