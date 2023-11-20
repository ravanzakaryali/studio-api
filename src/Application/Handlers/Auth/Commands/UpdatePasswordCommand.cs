namespace Space.Application.Handlers;

public class UpdatePasswordCommand : IRequest
{
    public Guid Key { get; set; }
    public string Password { get; set; } = null!;
}
internal class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public UpdatePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers.Where(c => c.Key == request.Key).FirstOrDefaultAsync() ??
            throw new NotFoundException(nameof(Worker), request.Key);
        if (worker.KeyExpirerDate < DateTime.UtcNow) throw new Exception();
        worker.Key = null;
        await _unitOfWork.UserService.PasswordAssignAsync(worker.Id, request.Password);
        await _spaceDbContext.SaveChangesAsync();
    }
}
