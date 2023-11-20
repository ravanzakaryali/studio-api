namespace Space.Application.Handlers;

public class RefreshPasswordCommand : IRequest
{
    public string Email { get; set; } = null!;
}

internal class RefreshPasswordCommandHandler : IRequestHandler<RefreshPasswordCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;

    public RefreshPasswordCommandHandler(
            IUnitOfWork unitOfWork,
            ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(RefreshPasswordCommand request, CancellationToken cancellationToken)
    {
        Worker worker = await _spaceDbContext.Workers.Where(c => c.Email == request.Email).FirstOrDefaultAsync() ??
            throw new NotFoundException(nameof(Worker), request.Email);

        if (worker.LastPasswordUpdateDate != null
            && (worker.LastPasswordUpdateDate.Value <= DateTime.UtcNow && worker.LastPasswordUpdateDate.Value.AddMinutes(15) >= DateTime.UtcNow))
            throw new DateTimeException($"Check after {worker.LastPasswordUpdateDate.Value.AddMinutes(15) - DateTime.UtcNow} minutes", worker.LastPasswordUpdateDate.Value.AddMinutes(15) - DateTime.UtcNow);

        worker.Key = Guid.NewGuid();
        worker.KeyExpirerDate = DateTime.UtcNow.AddMinutes(15);
        worker.LastPasswordUpdateDate = DateTime.UtcNow;
<<<<<<< HEAD
        string messaje = $"https://studio.code.az/admin/auth/confirmpassword/{worker.Key}";
        await _unitOfWork.EmailService.SendMessageAsync(messaje, worker.Email, "Şifrənizi dəyiştirin (no-reply)");
        await _unitOfWork.SaveChangesAsync(cancellationToken);
=======
        string messaje = $"https://dev-studio.code.az/admin/auth/confirmpassword/{worker.Key}";
        await _unitOfWork.EmailService.SendMessageAsync(messaje, worker.Email, "Şifrənizi dəyiştirin (no-reply)");
        await _spaceDbContext.SaveChangesAsync();
>>>>>>> 347b230a34d05d5ec4367901a704c1db3f19a102
    }
}
