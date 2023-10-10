namespace Space.Application.Handlers;

public class RefreshPasswordCommand : IRequest
{
    public string Email { get; set; } = null!;
}

internal class RefreshPasswordCommandHandler : IRequestHandler<RefreshPasswordCommand>
{
    readonly IUnitOfWork _unitOfWork;

    public RefreshPasswordCommandHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task Handle(RefreshPasswordCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _unitOfWork.WorkerRepository.GetAsync(w => w.Email == request.Email) ??
            throw new NotFoundException(nameof(Worker), request.Email);

        if (worker.LastPasswordUpdateDate != null
            && (worker.LastPasswordUpdateDate.Value <= DateTime.UtcNow && worker.LastPasswordUpdateDate.Value.AddMinutes(15) >= DateTime.UtcNow))
            throw new DateTimeException($"Check after {worker.LastPasswordUpdateDate.Value.AddMinutes(15) - DateTime.UtcNow} minutes", worker.LastPasswordUpdateDate.Value.AddMinutes(15) - DateTime.UtcNow);

        worker.Key = Guid.NewGuid();
        worker.KeyExpirerDate = DateTime.UtcNow.AddMinutes(15);
        worker.LastPasswordUpdateDate = DateTime.UtcNow;
        string messaje = $"https://dev-studio.code.az/admin/auth/confirmpassword/{worker.Key}";
        await _unitOfWork.EmailService.SendMessageAsync(messaje, worker.Email, "Şifrənizi dəyiştirin (no-reply)");
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
