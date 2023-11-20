namespace Space.Application.Handlers;

public class CreateClassModuleSessionCommand : IRequest
{
    public Guid ClassId { get; set; }
    public CreateClassModuleSessionRequestDto CreateClassModuleSessionDto { get; set; } = null!;
}
internal class CreateClassModuleSessionHandler : IRequestHandler<CreateClassModuleSessionCommand>
{
    private readonly IMediator _mediator;
    private readonly ISpaceDbContext _spaceDbContext;
    public CreateClassModuleSessionHandler(IMediator mediator, ISpaceDbContext spaceDbContext)
    {
        _mediator = mediator;
        _spaceDbContext = spaceDbContext;
    }

    //Todo: Request module change - High
    public async Task Handle(CreateClassModuleSessionCommand request, CancellationToken cancellationToken)
    {
        Task createModuleTask = _mediator.Send(new CreateClassModuleCommand()
        {
            ClassId = request.ClassId,
            CreateClassModule = request.CreateClassModuleSessionDto.Modules
        }, cancellationToken);

        Task createSessionTask = _mediator.Send(new CreateClassSessionCommand()
        {
            ClassId = request.ClassId,
            SessionId = request.CreateClassModuleSessionDto.SessionId
        }, cancellationToken);

        Task[] tasks = new Task[] { createModuleTask, createSessionTask };
        Task completedTask = await Task.WhenAny(tasks);

        if (completedTask.IsFaulted)
        {
            cancellationToken.ThrowIfCancellationRequested();
        }
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
