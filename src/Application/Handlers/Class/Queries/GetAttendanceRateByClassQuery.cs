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

        IEnumerable<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id)
            .Include(c => c.Attendances)
            .ToListAsync(cancellationToken: cancellationToken);

        return classTimeSheets.Where(c => c.Date.Month == month).Select(c => new GetAttendanceRateByClassDto()
        {
            Status = c.Status,
            TotalStudentsCount = c.Attendances.Count,
            AttendingStudentsCount = c.Attendances.Where(c => c.TotalAttendanceHours != 0).Count(),
            Date = c.Date,
        });
    }
}

