namespace Space.Application.Handlers.Commands;

public record UpdateClassCommand(
    Guid Id,
    string Name,
    Guid SessionId,
    Guid ProgramId,
    Guid? RoomId
    ) : IRequest<GetClassResponseDto>;
internal class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, GetClassResponseDto>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassRepository _classRepository;
    readonly IProgramRepository _programRepository;
    readonly ISessionRepository _sessionRepository;
    readonly IRoomRepository _roomRepository;


    public UpdateClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, IClassRepository classRepository, IProgramRepository programRepository, ISessionRepository sessionRepository, IRoomRepository roomRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classRepository = classRepository;
        _programRepository = programRepository;
        _sessionRepository = sessionRepository;
        _roomRepository = roomRepository;
    }

    public async Task<GetClassResponseDto> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _classRepository.GetAsync(c => c.Id == request.Id, tracking: false)
            ?? throw new NotFoundException(nameof(Class), request.Id);

        Program? program = await _programRepository.GetAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session session = await _sessionRepository.GetAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);

        if (request.RoomId == null)
        {
            Guid roomIdNonNullable = request.RoomId ?? Guid.Empty;
            Room room = await _roomRepository.GetAsync(roomIdNonNullable) ??
                throw new NotFoundException(nameof(Room), roomIdNonNullable);
        }

        Class newClass = _mapper.Map<Class>(request);

        newClass.Id = @class.Id;

        _classRepository.Update(newClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetClassResponseDto>(newClass);
    }
}
