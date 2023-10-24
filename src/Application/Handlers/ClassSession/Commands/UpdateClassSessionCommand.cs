namespace Space.Application.Handlers;

public record UpdateClassSessionCommand(Guid Id, DateTime Date, IEnumerable<UpdateClassSessionRequestDto> UpdateClassSessions) : IRequest;

internal class UpdateClassSessionCommandHandler : IRequestHandler<UpdateClassSessionCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;
    readonly IClassSessionRepository _classSessionRepository;
    readonly IClassRepository _classRepository;

    public UpdateClassSessionCommandHandler(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        IClassSessionRepository classSessionRepository,
        IClassRepository classRepository)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _classSessionRepository = classSessionRepository;
        _classRepository = classRepository;
    }

    public async Task Handle(UpdateClassSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _classRepository.GetAsync(c => c.Id == request.Id && c.ClassSessions.Any(c => c.Date == request.Date))
            ?? throw new NotFoundException(nameof(Class), request.Id);
        if (@class.ClassSessions.Count == 0) throw new NotFoundException(nameof(ClassSession), request.Date);

        if (@class.ClassSessions.Any(cs => cs.Status != null)) throw new Exception("Offline, Online and Cancelled not change");

        DateTime date = request.UpdateClassSessions.DistinctBy(c => c.ClassSessionDate).FirstOrDefault().ClassSessionDate;

        if (await _classSessionRepository.GetAsync(c => c.Date == date) != null) throw new Exception("Class Session already date");

        @class.ClassSessions = _mapper.Map<List<ClassSession>>(request.UpdateClassSessions);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
