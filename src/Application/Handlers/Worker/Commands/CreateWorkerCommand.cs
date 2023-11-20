using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record CreateWorkerCommand(string Name, string Surname, string Email) : IRequest<GetWorkerResponseDto>;

public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommand, GetWorkerResponseDto>
{
    readonly IMapper _mapper;
    readonly IUnitOfWork _unitOfWork;
    readonly UserManager<User> _userManager;
    readonly ISpaceDbContext _spaceDbContext;


    public CreateWorkerCommandHandler(
        IMapper mapper,
        UserManager<User> userManager,
        IUnitOfWork unitOfWork,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkerResponseDto> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
    {
        User worker = await _userManager.FindByEmailAsync(request.Email);
        if (worker != null) throw new AlreadyExistsException(nameof(Worker), request.Email);
        Worker newWorker = _mapper.Map<Worker>(request);
        await _unitOfWork.UserService.CreateWorkerAsync(newWorker);
        await _spaceDbContext.SaveChangesAsync();
        return _mapper.Map<GetWorkerResponseDto>(newWorker);
    }
}
