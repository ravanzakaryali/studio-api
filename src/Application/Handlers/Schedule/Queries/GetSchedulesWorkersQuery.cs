namespace Space.Application.Handlers;

public record GetSchedulesWorkersQuery : IRequest<IEnumerable<GetSchedulesWorkersResponseDto>>;

internal class GetSchedulesWorkersQueryHandler : IRequestHandler<GetSchedulesWorkersQuery, IEnumerable<GetSchedulesWorkersResponseDto>>
{
    readonly IMapper _mapper;
    readonly ISpaceDbContext _spaceDbContext;

    public GetSchedulesWorkersQueryHandler(
        IMapper mapper,
        ISpaceDbContext spaceDbContext)
    {
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetSchedulesWorkersResponseDto>> Handle(GetSchedulesWorkersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Worker> workers = await _spaceDbContext.Workers
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Class)
            .ToListAsync();

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
                DateOnly startDate = @class.StartDate;
                DateOnly endDate = @class.EndDate ?? DateOnly.FromDateTime(DateTime.Now);
                scheduleWorker.Schedules.Add(new GetSchedulesClassDto()
                {
                    Class = _mapper.Map<GetAllClassDto>(@class),
                    //EndDate = endDate,
                    //StartDate = startDate
                });
            }
            response.Add(scheduleWorker);
        }
        return response;
    }
}
