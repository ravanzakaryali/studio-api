namespace Space.Application.Handlers;

public record UpdateClassSessionCommand(int Id, DateTime Date, IEnumerable<UpdateClassSessionRequestDto> UpdateClassSessions) : IRequest;

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
        DateOnly requestDate = DateOnly.FromDateTime(request.Date);

        Class @class = await _spaceDbContext.Classes
            .Include(c => c.ClassSessions)
            .Where(c => c.Id == request.Id && c.ClassSessions.Any(c => c.Date == requestDate))
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        if (@class.ClassSessions.Count == 0) throw new NotFoundException(nameof(ClassTimeSheet), request.Date);

        DateOnly date = request.UpdateClassSessions.DistinctBy(c => c.ClassSessionDate).First().ClassSessionDate;

        if (await _spaceDbContext.ClassSessions.Where(c => c.Date == date).FirstOrDefaultAsync() != null)
            throw new Exception("Class Session already date");

        throw new NotFoundException();
    }
}
