using System;

namespace Space.Application.Handlers;


public record GetWorkerClassSessionsByClassQuery(Guid Id) : IRequest<GetWorkerClassSessionsByClassResponseDto>;



internal class GetWorkerClassSessionsByClassQueryHandler : IRequestHandler<GetWorkerClassSessionsByClassQuery, GetWorkerClassSessionsByClassResponseDto>
{

    readonly IUnitOfWork _unitOfWork;

    public GetWorkerClassSessionsByClassQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;

    }

    public async Task<GetWorkerClassSessionsByClassResponseDto> Handle(GetWorkerClassSessionsByClassQuery request, CancellationToken cancellationToken)
    {

        var classSessions = await _unitOfWork.ClassSessionRepository.GetAllAsync(q => q.ClassId
         == request.Id && q.Status != null, tracking: false, "Class");


        var @class = await _unitOfWork.ClassRepository.GetAsync(q => q.Id == request.Id);

        var response = new GetWorkerClassSessionsByClassResponseDto();

        response.ClassId = @class.Id;
        response.ClassName = @class.Name;


        var workerSessions = new List<GetWorkerClassSessionsDto>();

        var orderedClasssessions = classSessions.OrderByDescending(q => q.Date);

        int offlineHours = 0;
        int onlineHours = 0;
        int canceledHours = 0;

        foreach (var item in orderedClasssessions)
        {
            var workerSession = new GetWorkerClassSessionsDto();
            workerSession.Category = item.Category?.ToString();
            workerSession.Status = item.Status?.ToString();
            workerSession.TotalHours = item.TotalHour;
            workerSession.Date = item.Date;

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
