namespace Space.Application.Handlers;

public class EndAttendanceCommand : IRequest<GetClassTimeSheetResponseDto>
{
    public int ClassTimeSheetId { get; set; }
}
internal class EndAttendanceCommandHandler : IRequestHandler<EndAttendanceCommand, GetClassTimeSheetResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    public EndAttendanceCommandHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task<GetClassTimeSheetResponseDto> Handle(EndAttendanceCommand request, CancellationToken cancellationToken)
    {
        DateTime nowDate = DateTime.Now.AddHours(4);
        DateOnly now = DateOnly.FromDateTime(nowDate);
        TimeOnly nowTime = TimeOnly.FromDateTime(nowDate);

        ClassTimeSheet? classTimeSheet = await _spaceDbContext.ClassTimeSheets
                    .Where(c => c.Id == request.ClassTimeSheetId)
                    .Include(c => c.Class)
                    .FirstOrDefaultAsync();
        if (classTimeSheet == null) throw new NotFoundException(nameof(ClassTimeSheet), request.ClassTimeSheetId);


        if (classTimeSheet.EndTime != null) throw new BadHttpRequestException("Attendance already ended for this class");

        classTimeSheet.EndTime = nowTime;

        _spaceDbContext.ClassTimeSheets.Update(classTimeSheet);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
        return new GetClassTimeSheetResponseDto()
        {
            ClassTimeSheetId = classTimeSheet.Id,
            StartTime = classTimeSheet.StartTime,
            Category = classTimeSheet.Category,
            Class = new GetClassDto()
            {
                Id = classTimeSheet.Class.Id,
                Name = classTimeSheet.Class.Name,
            },
            EndTime = classTimeSheet.EndTime,
        };
    }
}