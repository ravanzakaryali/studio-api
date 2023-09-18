using System;

namespace Space.Application.Handlers;

public record GetWorkerClassSchedulesQuery() : IRequest<IEnumerable<GetWorkersSchedulesResponseDto>>;



internal class GetWorkerClassSchedulesQueryHandler : IRequestHandler<GetWorkerClassSchedulesQuery, IEnumerable<GetWorkersSchedulesResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;

    public GetWorkerClassSchedulesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetWorkersSchedulesResponseDto>> Handle(GetWorkerClassSchedulesQuery request, CancellationToken cancellationToken)
    {

        var workers = await _unitOfWork.WorkerRepository.GetAllAsync();

        var classSessions = await _unitOfWork.ClassSessionRepository.GetAllAsync(q => q.IsActive, tracking: false, "Worker");
        var classes = await _unitOfWork.ClassRepository.GetAllAsync();


        var response = new List<GetWorkersSchedulesResponseDto>();
        foreach (var item in workers)
        {
            var model = new GetWorkersSchedulesResponseDto();
            model.WorkerName = item.Name + " " + item.Surname;



            var workerClassSchedules = new List<GetWorkerClassSchedulesResponseDto>();
            foreach (var @class in classes)
            {
                //var classSessionStart = classSessions.Where(q => q.ClassId == @class.Id && q.WorkerId == item.Id).OrderBy(q => q.Date)?.Take(1).FirstOrDefault();
                //var classSessionEnd = classSessions.Where(q => q.ClassId == @class.Id && q.WorkerId == item.Id).OrderByDescending(q => q.Date)?.Take(1).FirstOrDefault();


               
                //if (classSessionStart != null && classSessionEnd != null)
                //{
                //    var workerClassSchedule = new GetWorkerClassSchedulesResponseDto();
                //    workerClassSchedule.ClassName = @class.Name;
                //    workerClassSchedule.StartDate = classSessionStart.Date;
                //    workerClassSchedule.EndDate = classSessionEnd.Date;
                //    workerClassSchedule.ClassColor = @class.Program?.Color;

                //    workerClassSchedules.Add(workerClassSchedule);
                //}

           
               
            }


            model.ClassSchedules = workerClassSchedules;
            response.Add(model);

        }

        return response;
    }
}