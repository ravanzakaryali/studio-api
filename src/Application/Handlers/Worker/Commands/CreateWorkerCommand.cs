namespace Space.Application.Handlers;

public record CreateWorkerCommand(string Name, string Surname, string Email, IEnumerable<int>? GroupsId) : IRequest<GetWorkerResponseDto>;

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
        Worker newWorker = new()
        {
            Name = request.Name,
            Surname = request.Surname,
            Email = request.Email,
            UserName = request.Email,
        };
        if (request.GroupsId != null)
        {
            List<PermissionGroup> permissionGroups = await _spaceDbContext.PermissionGroups
                                .Where(c => request.GroupsId.Contains(c.Id))
                                .ToListAsync();
            if (permissionGroups.Count != request.GroupsId.Count())
                throw new NotFoundException(nameof(PermissionGroup), request.GroupsId.Except(permissionGroups.Select(c => c.Id)).FirstOrDefault());
            foreach (int groupId in request.GroupsId)
            {
                PermissionGroup permissionGroup = permissionGroups.FirstOrDefault(p => p.Id == groupId) ??
                    throw new NotFoundException(nameof(PermissionGroup), groupId);

                newWorker.PermissionGroups.Add(permissionGroup);
            }
        }

        await _unitOfWork.UserService.CreateWorkerAsync(newWorker);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWorkerResponseDto>(newWorker);
    }
}
