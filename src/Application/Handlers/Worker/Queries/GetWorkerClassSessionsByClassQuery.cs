using System;

namespace Space.Application.Handlers;


public record GetWorkerClassSessionsByClassQuery(Guid Id) : IRequest<GetWorkerClassSessionsByClassResponseDto>;



internal class GetWorkerClassSessionsByClassQueryHandler : IRequestHandler<GetWorkerClassSessionsByClassQuery, GetWorkerClassSessionsByClassResponseDto>
{

    readonly IClassSessionRepository _classSessionRepository;
    readonly IClassRepository _classRepository;

    public GetWorkerClassSessionsByClassQueryHandler(
        IClassSessionRepository classSessionRepository,
        IClassRepository classRepository)
    {
        _classSessionRepository = classSessionRepository;
        _classRepository = classRepository;
    }

    public async Task<GetWorkerClassSessionsByClassResponseDto> Handle(GetWorkerClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {

        IEnumerable<ClassSession> classSessions = await _classSessionRepository.GetAllAsync(q => q.ClassId
         == request.Id && q.Status != null, tracking: false, "Class");


        Class @class = await _classRepository.GetAsync(q => q.Id == request.Id) ?? throw new NotFoundException(nameof(Class), request.Id);
        GetWorkerClassSessionsByClassResponseDto response = new()
        {
            ClassId = @class.Id,
            ClassName = @class.Name
        };


        var workerSessions = new List<GetWorkerClassSessionsDto>();

        var orderedClasssessions = classSessions.OrderByDescending(q => q.Date);

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
