

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


        return programs.Select(program =>
        {
            var filterClassSession = classSessions
                        .Where(cs => cs.Class.ProgramId == program.Id && cs.Date.Month == (int)request.Month && cs.Date.Year == request.Year).ToList();

            double totalAttendace = 0;
            if (classSessions.Where(classSession => classSession.Class.ProgramId == program.Id).Any())
            {
                totalAttendace = (double)filterClassSession.Where(c => c.ClassTimeSheetId == null).Count() /
                                filterClassSession.Count * 100;
            }
            return new GetUnMarkedAttendancesByProgramsDto()
            {
                Program = new GetProgramResponseDto()
                {
                    Id = program.Id,
                    Name = program.Name,
                },
                UnMarkedAttendancesCount = filterClassSession.Where(c => c.ClassTimeSheetId == null).DistinctBy(cs => cs.ClassId).Count(),
                TotalUnMarkedAttendancesCount = filterClassSession.Where(c => c.ClassTimeSheetId == null).Count(),
                TotalAttendancePercentage = Math.Round(totalAttendace, MidpointRounding.AwayFromZero)
            };
        });
    }
}
