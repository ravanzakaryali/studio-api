

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
            .Include(p => p.Classes)
            .ThenInclude(c => c.ClassSessions)
            .ToListAsync(cancellationToken: cancellationToken);
        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Include(c => c.Class)
            .Where(c => c.Date <= dateNow)
            .ToListAsync(cancellationToken: cancellationToken);


        return programs.Select(c =>
        {
            double totalAttendace = 0;
            if (classSessions.Where(cs => cs.Class.ProgramId == c.Id).Any())
            {
                totalAttendace = ((double)classSessions.Where(cs => cs.Class.ProgramId == c.Id && cs.ClassTimeSheetId != null).Count() / classSessions.Where(cs => cs.Class.ProgramId == c.Id).Count()) * 100;
            }
            return new GetUnMarkedAttendancesByProgramsDto()
            {
                Program = new GetProgramResponseDto()
                {
                    Id = c.Id,
                    Name = c.Name,
                },
                UnMarkedAttendancesCount = classSessions.Where(cs => cs.Class.ProgramId == c.Id && cs.ClassTimeSheetId == null).DistinctBy(cs => cs.ClassId).Count(),
                TotalUnMarkedAttendancesCount = classSessions.Where(cs => cs.Class.ProgramId == c.Id && cs.ClassTimeSheetId == null).Count(),
                TotalAttendancePercentage = Math.Round(totalAttendace, MidpointRounding.AwayFromZero)
            };
        });
    }
}
