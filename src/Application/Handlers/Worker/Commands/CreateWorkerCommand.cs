using Microsoft.AspNetCore.Identity;

namespace Space.Application.Handlers;

public record CreateWorkerCommand(string Name,string Surname,string Email) : IRequest<GetWorkerResponseDto>;

public class CreateWorkerCommandHandler : IRequestHandler<CreateWorkerCommand, GetWorkerResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly UserManager<User> _userManager;


    public CreateWorkerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _userManager = userManager;
    }

    public async Task<GetWorkerResponseDto> Handle(CreateWorkerCommand request, CancellationToken cancellationToken)
    {
        User worker = await _userManager.FindByEmailAsync(request.Email);
        if (worker != null) throw new AlreadyExistsException(nameof(Worker), request.Email);
        Worker newWorker = _mapper.Map<Worker>(request);
        await _unitOfWork.UserService.CreateWorkerAsync(newWorker);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWorkerResponseDto>(newWorker);
    }
}
