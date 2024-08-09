
public class GetAttendnaceSessionByClassQuery : IRequest<GetAttendanceSessionDto>
{
    public int ClassId { get; set; }
    public WorkerType WorkerType { get; set; }
}
internal class GetAttendnaceSessionByClassQueryHandler : IRequestHandler<GetAttendnaceSessionByClassQuery, GetAttendanceSessionDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMediator _mediator;
    readonly ICurrentUserService _currentUserService;

    public GetAttendnaceSessionByClassQueryHandler(ISpaceDbContext spaceDbContext, ICurrentUserService currentUserService, IMediator mediator)
    {
        _spaceDbContext = spaceDbContext;
        _currentUserService = currentUserService;
        _mediator = mediator;
    }

    public async Task<GetAttendanceSessionDto> Handle(GetAttendnaceSessionByClassQuery request, CancellationToken cancellationToken)
    {
        int loginUserId = int.Parse(_currentUserService.UserId ?? throw new UnauthorizedAccessException());


        DateTime dateTimeNow = DateTime.UtcNow.AddHours(4);
        DateOnly dateNow = DateOnly.FromDateTime(dateTimeNow);

        Class? @class = await _spaceDbContext.Classes.FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken) ??
            throw new NotFoundException(nameof(Class), request.ClassId);

        ClassSessionCategory category = ClassSessionCategory.Theoric;

        if (request.WorkerType == WorkerType.Mentor)
        {
            category = ClassSessionCategory.Lab;
        }



        ClassTimeSheet classTimeSheets = await _spaceDbContext.ClassTimeSheets
                        .Where(c => c.ClassId == @class.Id && c.Date == dateNow && c.Category == category)
                        .Include(c => c.HeldModules)
                        .ThenInclude(hm => hm.Module)
                        .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(ClassTimeSheet), request.ClassId);


        IEnumerable<GetAllStudentByClassResponseDto> attendanceStudents = await _mediator.Send(new GetAllStudentsByClassQuery(request.ClassId, dateTimeNow));


        GetAttendanceSessionDto response = new()
        {
            Class = new GetClassAttendanceDto()
            {
                Id = @class.Id,
                Name = @class.Name,
            },
            ClassTimeSheetId = classTimeSheets.Id,
            EndTime = classTimeSheets.EndTime,

            StartTime = classTimeSheets.StartTime,
            Category = classTimeSheets.Category,
            HeldModules = classTimeSheets.HeldModules.Select(hm => new GetHeldModulesDto()
            {
                Id = hm.Id,
                Name = hm.Module?.Name,
                TotalHours = hm.TotalHours,
                Version = hm.Module?.Version,

            }),

            Students = attendanceStudents
        };

        return response;

    }
}
