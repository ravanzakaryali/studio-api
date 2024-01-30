

using Space.Application.Enums;

namespace Space.Application.Handlers;

public class GetUnMarkedAttendancesByProgramsQuery : IRequest<IEnumerable<GetUnMarkedAttendancesByProgramsDto>>
{
    public MonthOfYear Month { get; set; }
    public int Year { get; set; }
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

        List<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Include(c => c.Class)
            .Where(c => c.Date <= dateNow)
            .ToListAsync(cancellationToken: cancellationToken);


        classSessions = classSessions
                                .Where(c => c.Date.Month == (int)request.Month && c.Date.Year == request.Year)
                                .ToList();


        return programs.Select(program =>
        {
            double totalAttendace = 0;
            if (classSessions.Where(classSession => classSession.Class.ProgramId == program.Id).Any())
            {
                totalAttendace = (double)classSessions.Where(classSession => classSession.Class.ProgramId == program.Id && classSession.ClassTimeSheetId != null).Count() /
                                classSessions.Where(classSession => classSession.Class.ProgramId == program.Id).Count() * 100;
            }
            return new GetUnMarkedAttendancesByProgramsDto()
            {
                Program = new GetProgramResponseDto()
                {
                    Id = program.Id,
                    Name = program.Name,
                },
                UnMarkedAttendancesCount = classSessions.Where(cs => cs.Class.ProgramId == program.Id && cs.ClassTimeSheetId == null).DistinctBy(cs => cs.ClassId).Count(),
                TotalUnMarkedAttendancesCount = classSessions.Where(cs => cs.Class.ProgramId == program.Id && cs.ClassTimeSheetId == null).Count(),
                TotalAttendancePercentage = Math.Round(totalAttendace, MidpointRounding.AwayFromZero)
            };
        });
    }
}
