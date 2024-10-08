﻿using Microsoft.Extensions.Configuration;

namespace Space.Application.Handlers;

public class RefreshPasswordCommand : IRequest
{
    public string Email { get; set; } = null!;
}

internal class RefreshPasswordCommandHandler : IRequestHandler<RefreshPasswordCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;
    readonly IConfiguration _configuration;

    public RefreshPasswordCommandHandler(
            IUnitOfWork unitOfWork,
            ISpaceDbContext spaceDbContext,
            IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
        _configuration = configuration;
    }

    public async Task Handle(RefreshPasswordCommand request, CancellationToken cancellationToken)
    {
        Worker worker = await _spaceDbContext.Workers.Where(c => c.Email == request.Email).FirstOrDefaultAsync() ??
            throw new NotFoundException(nameof(Worker), request.Email);

        if (worker.LastPasswordUpdateDate != null
            && worker.LastPasswordUpdateDate.Value <= DateTime.UtcNow && worker.LastPasswordUpdateDate.Value.AddMinutes(15) >= DateTime.UtcNow)
            throw new DateTimeException($"Check after {worker.LastPasswordUpdateDate.Value.AddMinutes(15) - DateTime.UtcNow} minutes", worker.LastPasswordUpdateDate.Value.AddMinutes(15) - DateTime.UtcNow);

        worker.Key = Guid.NewGuid();
        worker.KeyExpirerDate = DateTime.UtcNow.AddMinutes(15);
        worker.LastPasswordUpdateDate = DateTime.UtcNow;
        string messaje = $"{_configuration["App:ClientUrl"]}/auth/confirmpassword/{worker.Key}";
        await _unitOfWork.EmailService.SendMessageAsync(messaje, worker.Email, "EmailTemplate.html", "Şifrənizi dəyiştirin (no-reply)");
        await _spaceDbContext.SaveChangesAsync();
    }
}
