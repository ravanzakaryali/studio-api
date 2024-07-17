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
        DateOnly requestStartDate = DateOnly.FromDateTime(request.StartDate);
        DateOnly requestEndDate = DateOnly.FromDateTime(request.EndDate);

        List<Class> classes = await _spaceDbContext.Classes
            .Include(c => c.Session)
            .ThenInclude(s => s.Details)
            .Include(c => c.Studies)
            .Where(c => c.ProgramId == program.Id)
            .ToListAsync(cancellationToken: cancellationToken);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(cts => cts.Date >= requestStartDate && cts.Date <= requestEndDate)
            .Include(cts => cts.Attendances)
            .ToListAsync(cancellationToken);

        List<GetUnmarkedAttedanceClassesByProgramResponseDto> response = new();
        List<AvarageClassDto> list = new();

        foreach (Class classEntity in classes)
        {

            List<DateOnly> relevantDates = new();
            DateOnly? lastDate = null;

            DateOnly startDateClass = classEntity.StartDate > requestStartDate ? classEntity.StartDate : requestStartDate;
            DateOnly? endDateClass = classEntity.EndDate is not null && classEntity.EndDate < requestEndDate ? classEntity.EndDate : requestEndDate;

            for (DateOnly date = startDateClass; date <= endDateClass; date = date.AddDays(1))
            {
                DayOfWeek dayOfWeek = date.DayOfWeek;
                bool hasSession = classEntity.Session.Details.Any(sd => sd.DayOfWeek == dayOfWeek);
                if (hasSession)
                {
                    relevantDates.Add(date);
                }
            }

            foreach (DateOnly date in relevantDates.Where(date => date < DateOnly.FromDateTime(DateTime.Now)))
            {
                ClassTimeSheet? classTimeSheet = classTimeSheets
                    .FirstOrDefault(cts => cts.ClassId == classEntity.Id && cts.Date == date);

                if (classTimeSheet == null)
                {
                    lastDate = date;
                    // double totalAttendance = classTimeSheet.Attendances.Average(c => c.TotalAttendanceHours);
                    list.Add(new AvarageClassDto()
                    {
                        ClassId = classEntity.Id,

                    });
                    response.Add(new GetUnmarkedAttedanceClassesByProgramResponseDto()
                    {
                        StudentsCount = classEntity.Studies.Count,
                        Class = new GetClassDto()
                        {
                            Name = classEntity.Name,
                            Id = classEntity.Id,
                        },
                        LastDate = lastDate,
                        UnMarkDays = list.Where(l => l.ClassId == classEntity.Id).Count(),

                        AttendancePercentage = Math.Round(list.Where(l => l.ClassId == classEntity.Id).Any() ?
                                       list.Where(l => l.ClassId == classEntity.Id).Average(a => a.AverageHours) :
                                       0, 2),
                    });
                }
            }
        }


        return response.OrderByDescending(c => c.UnMarkDays).DistinctBy(c => c.Class.Id);
    }
}
