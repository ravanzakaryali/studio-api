using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Space.Application.Enums;

namespace Space.Application.Handlers;

public class GetAttendanceRateByClassQuery : IRequest<IEnumerable<GetAttendanceRateByClassDto>>
{
    public int Id { get; set; }
    public MonthOfYear MonthOfYear { get; set; }
    public int Year { get; set; }
}
internal class GetAttendanceRateByClassHandler : IRequestHandler<GetAttendanceRateByClassQuery, IEnumerable<GetAttendanceRateByClassDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAttendanceRateByClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAttendanceRateByClassDto>> Handle(GetAttendanceRateByClassQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);
        int month = (int)request.MonthOfYear;

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);

        IEnumerable<ClassSession> classSessions = await _spaceDbContext.ClassSessions
            .Include(c => c.ClassTimeSheet)
            .ThenInclude(c => c!.Attendances)
            .Where(c => c.ClassId == @class.Id)
            .ToListAsync(cancellationToken: cancellationToken);


        return classSessions.Where(c => c.Date.Month == month).Select(c => new GetAttendanceRateByClassDto()
        {
            Status = c.ClassTimeSheet == null && c.Status != ClassSessionStatus.Cancelled ? null : c.Status,
            TotalStudentsCount = c.ClassTimeSheet?.Attendances.Count,
            AttendingStudentsCount = c.ClassTimeSheet?.Attendances.Where(c => c.TotalAttendanceHours != 0).Count(),
            Date = c.Date,
        }).OrderBy(c => c.Date).DistinctBy(c => c.Date);
    }
}

