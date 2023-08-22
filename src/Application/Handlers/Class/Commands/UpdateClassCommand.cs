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

    public UpdateClassCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<GetClassResponseDto> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        Class? @class = await _unitOfWork.ClassRepository.GetAsync(c => c.Id == request.Id, tracking: false)
            ?? throw new NotFoundException(nameof(Class), request.Id);

        Program? program = await _unitOfWork.ProgramRepository.GetAsync(request.ProgramId) ??
            throw new NotFoundException(nameof(Program), request.ProgramId);

        Session session = await _unitOfWork.SessionRepository.GetAsync(request.SessionId) ??
            throw new NotFoundException(nameof(Session), request.SessionId);

        if (request.RoomId == null)
        {
            Guid roomIdNonNullable = request.RoomId ?? Guid.Empty;
            Room room = await _unitOfWork.RoomRepository.GetAsync(roomIdNonNullable) ??
                throw new NotFoundException(nameof(Room), roomIdNonNullable);
        }

        Class newClass = _mapper.Map<Class>(request);

        newClass.Id = @class.Id;

        _unitOfWork.ClassRepository.Update(newClass);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return _mapper.Map<GetClassResponseDto>(newClass);
    }
}
