using System;

namespace Space.Application.Handlers
{
    public record GetAllWorkersWithDetailsQuery : IRequest<IEnumerable<GetAllWorkersWithDetailsResponseDto>>;


    internal class GetAllWorkersWithDetailsQueryHandler : IRequestHandler<GetAllWorkersWithDetailsQuery, IEnumerable<GetAllWorkersWithDetailsResponseDto>>
    {
        readonly IWorkerRepository _workerRepository;
        readonly IClassModulesWorkerRepository _classModulesWorkerRepository;


        public GetAllWorkersWithDetailsQueryHandler(IClassModulesWorkerRepository classModulesWorkerRepository, IWorkerRepository workerRepository)
        {
            _classModulesWorkerRepository = classModulesWorkerRepository;
            _workerRepository = workerRepository;
        }

        public async Task<IEnumerable<GetAllWorkersWithDetailsResponseDto>> Handle(GetAllWorkersWithDetailsQuery request, CancellationToken cancellationToken)
        {

            var workers = await _workerRepository.GetAllAsync(predicate: null, tracking: false, "UserRoles");
            var workersClasses = await _classModulesWorkerRepository.GetAllAsync(predicate: null, tracking: false, "Class");

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
                //model.Roles = item.UserRoles;

                IEnumerable<ClassModulesWorker> data = workersClasses.Where(q => q.WorkerId == item.Id);
                List<WorkersClassesDto> workersClassesDtos = new();

                foreach (ClassModulesWorker workerClass in data)
                {
                    //aynı class var mı kontrol? code review yapılmalı

                    if (!workersClassesDtos.Any(q => q.ClassId == workerClass.Class.Id))
                    {
                        WorkersClassesDto workersClassesDto = new()
                        {
                            ClassId = workerClass.Class.Id,
                            ClassName = workerClass.Class.Name,
                            IsOpen = workerClass.Class.EndDate > DateTime.Now,
                            StartDate = workerClass.Class.StartDate,
                            EndDate = workerClass.Class.EndDate
                        };

                        workersClassesDtos.Add(workersClassesDto);
                    }

                }

                model.WorkerClasses = workersClassesDtos;

                response.Add(model);

            }


            return response;
        }
    }

}

