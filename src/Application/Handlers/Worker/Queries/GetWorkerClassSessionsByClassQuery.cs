namespace Space.Application.Handlers;

public record GetWorkerClassSessionsByClassQuery(Guid Id) : IRequest<GetWorkerClassSessionsByClassResponseDto>;


internal class GetWorkerClassSessionsByClassQueryHandler : IRequestHandler<GetWorkerClassSessionsByClassQuery, GetWorkerClassSessionsByClassResponseDto>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetWorkerClassSessionsByClassQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<GetWorkerClassSessionsByClassResponseDto> Handle(GetWorkerClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {

        IEnumerable<ClassTimeSheet> classSessions = await _spaceDbContext.ClassSessions
            .Include(c => c.Class)
            .Where(q => q.ClassId == request.Id && q.Status != null)
            .ToListAsync();


        Class @class = await _spaceDbContext.Classes.Where(q => q.Id == request.Id).FirstOrDefaultAsync() ??
            throw new NotFoundException(nameof(Class), request.Id);

        GetWorkerClassSessionsByClassResponseDto response = new()
        {
            ClassId = @class.Id,
            ClassName = @class.Name
        };

        List<GetWorkerClassSessionsDto> workerSessions = new List<GetWorkerClassSessionsDto>();

        IOrderedEnumerable<ClassTimeSheet> orderedClasssessions = classSessions.OrderByDescending(q => q.Date);

        int offlineHours = 0;
        int onlineHours = 0;
        int canceledHours = 0;

        foreach (var item in orderedClasssessions)
        {
            var workerSession = new GetWorkerClassSessionsDto
            {
                Category = item.Category?.ToString(),
                Status = item.Status?.ToString(),
                TotalHours = item.TotalHour,
                Date = item.Date
            };

            if (item.Status == ClassSessionStatus.Online)
                onlineHours += item.TotalHour;
            if (item.Status == ClassSessionStatus.Offline)
                offlineHours += item.TotalHour;
            if (item.Status == ClassSessionStatus.Cancelled)
                canceledHours += item.TotalHour;

            workerSessions.Add(workerSession);

        }

        response.WorkerClassSessions = workerSessions;
        response.CancaledHours = canceledHours;
        response.OnlineHours = onlineHours;
        response.OfflineHours = offlineHours;

        return response;
    }
}
