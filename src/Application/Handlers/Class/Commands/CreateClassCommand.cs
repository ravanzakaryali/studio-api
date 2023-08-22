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

    public CreateClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetWithIncludeClassResponseDto> Handle(CreateClassCommand request, CancellationToken cancellationToken)
    {
        Program? program = await _unitOfWork.ProgramRepository.GetAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session? session = await _unitOfWork.SessionRepository.GetAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);


        if (request.RoomId != null)
        {
        Guid roomIdNonNullable = request.RoomId ?? Guid.Empty;
            Room room = await _unitOfWork.RoomRepository.GetAsync(roomIdNonNullable) ??
                throw new NotFoundException(nameof(Room), roomIdNonNullable);
        }

        Class @class = await _unitOfWork.ClassRepository.AddAsync(_mapper.Map<Class>(request));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetWithIncludeClassResponseDto>(@class);
    }
}
