namespace Space.Application.Handlers;

public class GetClassDetailQuery : IRequest<GetClassDetailResponse>
{
    public int Id { get; set; }
}

internal class GetClassDetaulQueryHandler : IRequestHandler<GetClassDetailQuery, GetClassDetailResponse>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;

    public GetClassDetaulQueryHandler(ISpaceDbContext spaceDbContext, IUnitOfWork unitOfWork)
    {
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetClassDetailResponse> Handle(GetClassDetailQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Program)
            .Include(c => c.Session)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

        List<ClassTimeSheet> classTimeSheets = await _spaceDbContext.ClassTimeSheets
            .Where(c => c.ClassId == @class.Id && c.Status != ClassSessionStatus.Cancelled)
            .Include(c => c.Attendances)
            .ToListAsync(cancellationToken: cancellationToken);

        List<double> list = new();
        foreach (ClassTimeSheet? item in classTimeSheets.Where(c => c.Attendances.Count > 0))
        {
            var total = item.TotalHours;
            var totalAttendance = item.Attendances.Average(c => c.TotalAttendanceHours);
            // list.Add(totalAttendance * 100 / total);
            //todo: fix this
        }

        DateOnly startDate = @class.StartDate;
        DateOnly? endDate = @class.EndDate;

        
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
            // CurrentHours = classTimeSheets
            // .Where(c => c.Status != ClassSessionStatus.Cancelled)
            // .Sum(c => c.TotalHours),
            TotalHours = @class.Program.TotalHours,
            Name = @class.Name,
            AttendanceRate = Math.Round(list.Count > 0 ? list.Average() : 0, 2),
            EndDate = endDate,
            StartDate = startDate
        };
    }
}
