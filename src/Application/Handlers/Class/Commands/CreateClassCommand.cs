namespace Space.Application.Handlers.Commands;

public record CreateClassCommand(
        string Name,
        Guid ProgramId,
        Guid SessionId,
        Guid? RoomId) : IRequest<GetWithIncludeClassResponseDto>;
internal class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, GetWithIncludeClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IProgramRepository _programRepository;
    readonly ISessionRepository _sessionRepository;
    readonly IRoomRepository _roomRepository;
    readonly IClassRepository _classRepository;

    public CreateClassCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IProgramRepository programRepository,
        ISessionRepository sessionRepository,
        IRoomRepository roomRepository,
        IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _programRepository = programRepository;
        _sessionRepository = sessionRepository;
        _roomRepository = roomRepository;
        _classRepository = classRepository;
    }

    public async Task<GetWithIncludeClassResponseDto> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        Program? program = await _programRepository.GetAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session? session = await _sessionRepository.GetAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);


        if (request.RoomId != null)
        {
            Guid roomIdNonNullable = request.RoomId ?? Guid.Empty;
            Room room = await _roomRepository.GetAsync(roomIdNonNullable) ??
                throw new NotFoundException(nameof(Room), roomIdNonNullable);
        }

        Class @class = await _classRepository.AddAsync(_mapper.Map<Class>(request));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWithIncludeClassResponseDto>(@class);
    }
}
