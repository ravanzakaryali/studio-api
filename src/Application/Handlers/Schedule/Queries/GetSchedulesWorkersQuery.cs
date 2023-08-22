using System.Security.Cryptography.X509Certificates;

namespace Space.Application.Handlers;

public record GetSchedulesWorkersQuery : IRequest<IEnumerable<GetSchedulesWorkersResponseDto>>;

internal class GetSchedulesWorkersQueryHandler : IRequestHandler<GetSchedulesWorkersQuery, IEnumerable<GetSchedulesWorkersResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IMapper _mapper;

    public GetSchedulesWorkersQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GetSchedulesWorkersResponseDto>> Handle(GetSchedulesWorkersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Worker> workers = await _unitOfWork.WorkerRepository.GetAllAsync(predicate: null, tracking: false, "ClassModulesWorkers.Class.ClassSessions");

        List<GetSchedulesWorkersResponseDto> response = new();
        foreach (Worker worker in workers)
        {
            GetSchedulesWorkersResponseDto scheduleWorker = new()
            {
                Worker = _mapper.Map<GetWorkerResponseDto>(worker)
            };

            List<Class> workerClass = worker.ClassModulesWorkers.DistinctBy(c => c.ClassId).Select(c => c.Class).ToList();
            foreach (Class @class in workerClass)
            {
                if (@class.ClassSessions.Count == 0) continue;
                DateTime startDate = @class.ClassSessions.Min(c => c.Date);
                DateTime endDate = @class.ClassSessions.Max(c => c.Date);
                scheduleWorker.Schedules.Add(new GetSchedulesClassDto()
                {
                    Class = _mapper.Map<GetAllClassDto>(@class),
                    EndDate = endDate,
                    StartDate = startDate
                });
            }
            response.Add(scheduleWorker);
        }
        return response;
    }
}
