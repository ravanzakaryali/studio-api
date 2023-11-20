namespace Space.Application.Handlers;

public record UpdateClassSessionCommand(Guid Id, DateTime Date, IEnumerable<UpdateClassSessionRequestDto> UpdateClassSessions) : IRequest;

internal class UpdateClassSessionCommandHandler : IRequestHandler<UpdateClassSessionCommand>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public UpdateClassSessionCommandHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(UpdateClassSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id && c.ClassSessions.Any(c => c.Date == request.Date))
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

        if (@class.ClassSessions.Count == 0) throw new NotFoundException(nameof(ClassSession), request.Date);

        if (@class.ClassSessions.Any(cs => cs.Status != null)) throw new Exception("Offline, Online and Cancelled not change");

        DateTime date = request.UpdateClassSessions.DistinctBy(c => c.ClassSessionDate).First().ClassSessionDate;

        if (await _spaceDbContext.ClassSessions.Where(c => c.Date == date).FirstOrDefaultAsync() != null) throw new Exception("Class Session already date");

        @class.ClassSessions = _mapper.Map<List<ClassSession>>(request.UpdateClassSessions);
        await _spaceDbContext.SaveChangesAsync();
    }
}
