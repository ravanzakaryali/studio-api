namespace Space.Application.Handlers;

public class GetClassDetailQuery : IRequest<GetClassDetailResponse>
{
    public Guid Id { get; set; }
}

internal class GetClassDetaulQueryHandler : IRequestHandler<GetClassDetailQuery, GetClassDetailResponse>
{
    readonly ISpaceDbContext _dbContext;

    public GetClassDetaulQueryHandler(ISpaceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetClassDetailResponse> Handle(GetClassDetailQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _dbContext.Classes.Include(c => c.Program)
                                                .Include(c => c.Session)
                                                .FirstOrDefaultAsync(c => c.Id == request.Id)
                ?? throw new NotFoundException(nameof(Class), request.Id);


        List<ClassSession> classSessoins = await _dbContext.ClassSessions
                                                          .Where(c => c.ClassId == @class.Id && c.Status != null && c.Status != ClassSessionStatus.Cancelled)
                                                          .Include(c => c.Attendances)
                                                          .ToListAsync();
        List<double> list = new();
        foreach (ClassSession? item in classSessoins.Where(c => c.Attendances.Count > 0))
        {
            var total = item.TotalHour;
            var totalAttendance = item.Attendances.Average(c => c.TotalAttendanceHours);
            list.Add((totalAttendance * 100) / total);
        }

        return new GetClassDetailResponse()
        {
            Session = new GetSessionResponseDto()
            {
                Id = @class.Session.Id,
                Name = @class.Session.Name,
            },
            Program = new GetProgramResponseDto()
            {
                Id = @class.Program.Id,
                Name = @class.Program.Name,
            },
            Name = @class.Name,
            AttendanceRate = Math.Round(list.Count > 0 ? list.Average() : 0, 2),
            EndDate = @class.EndDate,
            StartDate = @class.StartDate
        };
    }
}
