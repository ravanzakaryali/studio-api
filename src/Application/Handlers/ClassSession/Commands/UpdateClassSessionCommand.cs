namespace Space.Application.Handlers;

public record UpdateClassSessionCommand(Guid Id, DateTime Date, IEnumerable<UpdateClassSessionRequestDto> UpdateClassSessions) : IRequest;

internal class UpdateClassSessionCommandHandler : IRequestHandler<UpdateClassSessionCommand>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public UpdateClassSessionCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateClassSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _unitOfWork.ClassRepository.GetAsync(c => c.Id == request.Id && c.ClassSessions.Any(c => c.Date == request.Date))
            ?? throw new NotFoundException(nameof(Class), request.Id);
        if (@class.ClassSessions.Count == 0) throw new NotFoundException(nameof(ClassSession), request.Date);

        if (@class.ClassSessions.Any(cs => cs.Status != null)) throw new Exception("Offline, Online and Cancelled not change");

        DateTime date = request.UpdateClassSessions.DistinctBy(c => c.ClassSessionDate).FirstOrDefault().ClassSessionDate;

        if (await _unitOfWork.ClassSessionRepository.GetAsync(c => c.Date == date) != null) throw new Exception("Class Session already date");

        @class.ClassSessions = _mapper.Map<List<ClassSession>>(request.UpdateClassSessions);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
