using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public class UpdateWorkerCommand : IRequest<GetWorkerResponseDto>
{
    public Guid Id { get; set; }
    public WorkerDto Worker { get; set; } = null!;
}
internal class UpdateWorkerCommandHandler : IRequestHandler<UpdateWorkerCommand, GetWorkerResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly UserManager<User> _userManager;

    public UpdateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<GetWorkerResponseDto> Handle(UpdateWorkerCommand request, CancellationToken cancellationToken)
    {
        Worker? worker = await _unitOfWork.WorkerRepository.GetAsync(request.Id)
            ?? throw new NotFoundException(nameof(Worker), request.Id);

        worker.Name = request.Worker.Name;
        worker.Surname = request.Worker.Surname;
        worker.Email = request.Worker.Email;
        worker.NormalizedEmail = request.Worker.Email.Normalize();
        worker.UserName = request.Worker.Email;
        worker.NormalizedUserName = request.Worker.Email.Normalize();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWorkerResponseDto>(worker);
    }
}
