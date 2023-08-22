using System;

namespace Space.Application.Handlers
{
    public record GetAllWorkersWithDetailsQuery : IRequest<IEnumerable<GetAllWorkersWithDetailsResponseDto>>;


    internal class GetAllWorkersWithDetailsQueryHandler : IRequestHandler<GetAllWorkersWithDetailsQuery, IEnumerable<GetAllWorkersWithDetailsResponseDto>>
    {
        readonly IUnitOfWork _unitOfWork;


        public GetAllWorkersWithDetailsQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
 
        }

        public async Task<IEnumerable<GetAllWorkersWithDetailsResponseDto>> Handle(GetAllWorkersWithDetailsQuery request, CancellationToken cancellationToken)
        {

            var workers = await _unitOfWork.WorkerRepository.GetAllAsync(predicate: null, tracking: false, "UserRoles");
            var workersClasses = await _unitOfWork.ClassModulesWorkerRepository.GetAllAsync(predicate: null, tracking: false, "Class");

            var response = new List<GetAllWorkersWithDetailsResponseDto>();

            foreach (var item in workers)
            {
                var model = new GetAllWorkersWithDetailsResponseDto();

                model.EMail = item.Email;
                model.Name = item.Name;
                model.Surname = item.Surname;
                model.Id = item.Id;
                //model.Roles = item.UserRoles;

                var data = workersClasses.Where(q => q.WorkerId == item.Id);

                var workersClassesDtos = new List<WorkersClassesDto>();

                foreach (var workerClass in data)
                {
                    //aynı class var mı kontrol? code review yapılmalı

                    if(!workersClassesDtos.Any(q => q.ClassId == workerClass.Class.Id))
                    {
                        var workersClassesDto = new WorkersClassesDto();
                        workersClassesDto.ClassId = workerClass.Class.Id;
                        workersClassesDto.ClassName = workerClass.Class.Name;
                        workersClassesDto.IsOpen = workerClass.Class.EndDate > DateTime.Now ? true : false;
                        workersClassesDto.StartDate = workerClass.Class.StartDate;
                        workersClassesDto.EndDate = workerClass.Class.EndDate;



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

