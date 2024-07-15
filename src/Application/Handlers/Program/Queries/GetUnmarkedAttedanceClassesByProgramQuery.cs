using Space.Application.Enums;
using Space.Domain.Entities;

namespace Space.Application.Handlers;

public class GetUnmarkedAttedanceClassesByProgramQuery : IRequest<IEnumerable<GetUnmarkedAttedanceClassesByProgramResponseDto>>
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
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
        DateOnly startDate = DateOnly.FromDateTime(request.StartDate);
        DateOnly endDate = DateOnly.FromDateTime(request.EndDate);

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
            .GroupBy(c => c.ClassId)
            .Select(g =>
            {
                var sessions = g
                    .Where(cs => startDate <= cs.Date && cs.Date <= endDate && cs.Status != ClassSessionStatus.Cancelled)
                    .ToList();

                return new GetUnmarkedAttedanceClassesByProgramResponseDto()
                {
                    StudentsCount = g.First().Class.Studies.Count,
                    AttendancePercentage = Math.Round(list.Where(l => l.ClassId == g.Key).Any() ?
                                        list.Where(l => l.ClassId == g.Key).Average(a => a.AverageHours) :
                                        0, 2),
                    UnMarkDays = sessions.Count(s => s.ClassTimeSheet is null),
                    Class = new GetClassDto()
                    {
                        Id = g.Key,
                        Name = g.First().Class.Name
                    },
                    LastDate = sessions
                        .OrderByDescending(cs => cs.Date)
                        .FirstOrDefault()?.Date
                };
            })
            .Where(c => c.UnMarkDays != 0)
            .OrderByDescending(c => c.UnMarkDays)
            .ToList());

        return response;
    }
}
