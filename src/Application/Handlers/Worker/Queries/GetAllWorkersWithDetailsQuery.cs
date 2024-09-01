namespace Space.Application.Handlers;

public record GetAllWorkersWithDetailsQuery : IRequest<IEnumerable<GetAllWorkersWithDetailsResponseDto>>;

internal class GetAllWorkersWithDetailsQueryHandler : IRequestHandler<GetAllWorkersWithDetailsQuery, IEnumerable<GetAllWorkersWithDetailsResponseDto>>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetAllWorkersWithDetailsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllWorkersWithDetailsResponseDto>> Handle(GetAllWorkersWithDetailsQuery request, CancellationToken cancellationToken)
    {

        List<Space.Domain.Entities.Worker> workers = await _spaceDbContext.Workers.Include(c => c.UserRoles).ToListAsync();
        List<ClassModulesWorker> workersClasses = await _spaceDbContext.ClassModulesWorkers.Include(c => c.Class).ToListAsync();

        var response = new List<GetAllWorkersWithDetailsResponseDto>();

        foreach (var item in workers)
        {
            if (item == null) break;
            var model = new GetAllWorkersWithDetailsResponseDto
            {
                EMail = item.Email,
                Name = item.Name,
                Surname = item.Surname,
                Id = item.Id
            };

            IEnumerable<ClassModulesWorker> data = workersClasses.Where(q => q.WorkerId == item.Id);
            List<WorkersClassesDto> workersClassesDtos = new();

            foreach (ClassModulesWorker workerClass in data)
            {

                if (!workersClassesDtos.Any(q => q.ClassId == workerClass.Class.Id))
                {
                    WorkersClassesDto workersClassesDto = new()
                    {
                        ClassId = workerClass.Class.Id,
                        ClassName = workerClass.Class.Name,
                        IsOpen = workerClass.Class.EndDate > DateOnly.FromDateTime(DateTime.Now),
                        StartDate = workerClass.Class.StartDate,
                        EndDate = workerClass.Class.EndDate
                    };
                    if (workerClass.Class.EndDate < DateOnly.FromDateTime(DateTime.Now))
                    {
                        continue;
                    }

                    workersClassesDtos.Add(workersClassesDto);
                }

            }

            model.WorkerClasses = workersClassesDtos;

            response.Add(model);

        }


        return response;
    }
}

