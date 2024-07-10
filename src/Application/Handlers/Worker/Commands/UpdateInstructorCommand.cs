using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public class UpdateWorkerCommand : IRequest<GetWorkerResponseDto>
{
    public int Id { get; set; }
    public WorkerDto Worker { get; set; } = null!;
}
internal class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, GetWorkerResponseDto>
{
    readonly IMapper _mapper;
    readonly UserManager<User> _userManager;
    readonly ISpaceDbContext _spaceDbContext;

    public UpdateWorkerCommandHandler(
        IMapper mapper,
        UserManager<User> userManager,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _userManager = userManager;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkerResponseDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _spaceDbContext.Workers.FindAsync(request.Id) ??
            throw new NotFoundException(nameof(Worker), request.Id);

        Worker? workerFincode = await _spaceDbContext.Workers
            .FirstOrDefaultAsync(c => c.Fincode == request.Worker.Fincode);

        if (await _spaceDbContext.Workers.AnyAsync(c => c.Fincode == request.Worker.Fincode))
            throw new AlreadyExistsException("Worker with this fincode already exists");

        worker.Name = request.Worker.Name;
        worker.Surname = request.Worker.Surname;
        worker.Email = request.Worker.Email;
        worker.Fincode = request.Worker.Fincode;
        worker.NormalizedEmail = request.Worker.Email.Normalize();
        worker.UserName = request.Worker.Email;
        worker.NormalizedUserName = request.Worker.Email.Normalize();

        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetWorkerResponseDto>(worker);
    }
}
