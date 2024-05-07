
using Space.Application.Enums;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class GetUnmarkedAttedanceClassesByProgramQuery : IRequest<IEnumerable<GetUnmarkedAttedanceClassesByProgramResponseDto>>
{
    public int Id { get; set; }
    public MonthOfYear Month { get; set; }
    public int Year { get; set; }
    public int? Day { get; set; }
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

        List<ClassSession> classSessions = await _spaceDbContext
            .ClassSessions
            .Include(c => c.ClassTimeSheet)
            .ThenInclude(c => c!.Attendances)
            .Include(c => c.Class)
            .ThenInclude(c => c.Studies)
            .Where(c => c.Class.ProgramId == program.Id && c.Date < dateNow)
            .ToListAsync(cancellationToken: cancellationToken);


        List<GetUnmarkedAttedanceClassesByProgramResponseDto> response = new();
        List<AvarageClassDto> list = new();

        DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Now);

        foreach (ClassSession? item in classSessions.Where(c => c.ClassTimeSheet?.Attendances.Count > 0))
        {
            int total = item.TotalHours;
            double totalAttendance = item.ClassTimeSheet?.Attendances.Average(c => c.TotalAttendanceHours) ?? 0;
            list.Add(new AvarageClassDto()
            {
                AverageHours = totalAttendance * 100 / total,
                ClassId = item.ClassId,
            });
        }

        response.AddRange(classSessions

            .Select(c =>
            {
                IEnumerable<ClassSession> filterClassSession = classSessions
                                         .Where(cs => cs.ClassId == c.ClassId &&
                                                 cs.ClassTimeSheetId is null &&
                                                 cs.Date.Year == request.Year &&
                                                 cs.Date.Month == (int)request.Month &&
                                                 cs.Status != ClassSessionStatus.Cancelled);

                if (request.Day is not null)
                {
                    filterClassSession = filterClassSession.Where(cs => cs.Date.Day == request.Day);
                }

                return new GetUnmarkedAttedanceClassesByProgramResponseDto()
                {
                    StudentsCount = c.Class.Studies.Count,
                    AttendancePercentage = Math.Round(list.Where(l => l.ClassId == c.ClassId).Any() ?
                                    list.Where(l => l.ClassId == c.ClassId).Average(a => a.AverageHours) :
                                    0, 2),
                    UnMarkDays = filterClassSession.Count(),
                    Class = new GetClassDto()
                    {
                        Id = c.ClassId,
                        Name = c.Class.Name
                    },
                    LastDate = filterClassSession.OrderByDescending(cs => cs.Date)
                                         .FirstOrDefault()?.Date

                };
            })
            .Where(c => c.UnMarkDays != 0)
            .DistinctBy(c => c.Class.Id)
            .OrderByDescending(c => c.UnMarkDays)
            .ToList());

        return response.OrderByDescending(c => c.UnMarkDays);
    }

}
