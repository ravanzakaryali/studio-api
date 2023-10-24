namespace Space.Application.Handlers;

public class UpdatePasswordCommand : IRequest
{
    public Guid Key { get; set; }
    public string Password { get; set; } = null!;
}
internal class UpdatePasswordCommandHandler : IRequestHandler<UpdatePasswordCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IWorkerRepository _workerRepository;

    public UpdatePasswordCommandHandler(
        IUnitOfWork unitOfWork,
        IWorkerRepository workerRepository)
    {
        _unitOfWork = unitOfWork;
        _workerRepository = workerRepository;
    }

    public async Task Handle(UpdatePasswordCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _workerRepository.GetAsync(w => w.Key == request.Key) ??
            throw new NotFoundException(nameof(Worker), request.Key);
        if (worker.KeyExpirerDate < DateTime.UtcNow) throw new Exception();
        worker.Key = null;
        await _unitOfWork.UserService.PasswordAssignAsync(worker.Id, request.Password);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
