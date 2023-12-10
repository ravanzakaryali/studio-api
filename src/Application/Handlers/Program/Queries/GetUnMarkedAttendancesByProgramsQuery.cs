

namespace Space.Application.Handlers;

public class GetUnMarkedAttendancesByProgramsQuery : IRequest<IEnumerable<GetUnMarkedAttendancesByProgramsDto>>
{

}
internal class GetUnMarkedAttendancesByProgramsHandler : IRequestHandler<GetUnMarkedAttendancesByProgramsQuery, IEnumerable<GetUnMarkedAttendancesByProgramsDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetUnMarkedAttendancesByProgramsHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetUnMarkedAttendancesByProgramsDto>> Handle(GetUnMarkedAttendancesByProgramsQuery request, CancellationToken cancellationToken)
    {
        List<Program> programs = await _spaceDbContext.Programs
            .ToListAsync(cancellationToken: cancellationToken);
        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        List<ClassGenerateSession> classSessions = await _spaceDbContext.ClassGenerateSessions
            .Include(c => c.Class)
            .Where(c => c.Date <= dateNow)
            .ToListAsync(cancellationToken: cancellationToken);

        return programs.Select(c => new GetUnMarkedAttendancesByProgramsDto()
        {
            UnMarkedAttendancesCount = classSessions.DistinctBy(cs => cs.ClassId).Where(c => c.Class.ProgramId == c.Id && c.ClassTimeSheetId == null).Count(),
            TotalUnMarkedAttendancesCount = classSessions.Where(c => c.Class.ProgramId == c.Id && c.ClassTimeSheetId == null).Count(),
            TotalAttendance = (classSessions.Where(c => c.Class.ProgramId == c.Id).Count() / classSessions.Where(c => c.Class.ProgramId == c.Id && c.ClassTimeSheetId != null).Count()) * 100
        });
    }
}
