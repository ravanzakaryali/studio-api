using System.Security.Cryptography.X509Certificates;

namespace Space.Application.Handlers;

public record GetSchedulesWorkersQuery : IRequest<IEnumerable<GetSchedulesWorkersResponseDto>>;

internal class GetSchedulesWorkersQueryHandler : IRequestHandler<GetSchedulesWorkersQuery, IEnumerable<GetSchedulesWorkersResponseDto>>
{
    readonly IMapper _mapper;
    readonly IWorkerRepository _workerRepository;

    public GetSchedulesWorkersQueryHandler(IMapper mapper, IWorkerRepository workerRepository)
    {
        _mapper = mapper;
        _workerRepository = workerRepository;
    }

    public async Task<IEnumerable<GetSchedulesWorkersResponseDto>> Handle(GetSchedulesWorkersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Worker> workers = await _workerRepository.GetAllAsync(predicate: null, tracking: false, "ClassModulesWorkers.Class");

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
                DateTime startDate = @class.StartDate ?? DateTime.Now;
                DateTime endDate = @class.EndDate ?? DateTime.Now;
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
