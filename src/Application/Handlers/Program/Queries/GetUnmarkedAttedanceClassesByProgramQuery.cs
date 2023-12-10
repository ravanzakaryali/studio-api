
namespace Space.Application.Handlers;

public class GetUnmarkedAttedanceClassesByProgramQuery : IRequest<IEnumerable<GetUnmarkedAttedanceClassesByProgramResponseDto>>
{
    public Guid Id { get; set; }
}
internal class GetUnmarkedAttedanceClassesByProgramHandler : IRequestHandler<GetUnmarkedAttedanceClassesByProgramQuery, IEnumerable<GetUnmarkedAttedanceClassesByProgramResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetUnmarkedAttedanceClassesByProgramHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetUnmarkedAttedanceClassesByProgramResponseDto>> Handle(GetUnmarkedAttedanceClassesByProgramQuery request, CancellationToken cancellationToken)
    {
        Program program = await _spaceDbContext.Programs
            .Where(p => p.Id == request.Id).FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Program), request.Id);

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        List<ClassGenerateSession> classSessions = await _spaceDbContext
            .ClassGenerateSessions
            .Include(c => c.Class)
            .Where(c => c.Class.ProgramId == program.Id && c.ClassTimeSheetId == null && c.Date <= dateNow)
            .ToListAsync(cancellationToken: cancellationToken);
        throw new NotFoundException("Not found endpoint");

    }
}
