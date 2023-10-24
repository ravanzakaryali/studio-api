using System;

namespace Space.Application.Handlers;


public record GetWorkerGeneralReportQuery(Guid Id) : IRequest<GetWorkerGeneralReportResponseDto>;


internal class GetWorkerGeneralReportQueryHandler : IRequestHandler<GetWorkerGeneralReportQuery, GetWorkerGeneralReportResponseDto>
{

    readonly IUnitOfWork _unitOfWork;
    readonly IWorkerRepository _workerRepository;

    public GetWorkerGeneralReportQueryHandler(
        IUnitOfWork unitOfWork,
        IWorkerRepository workerRepository)
    {
        _unitOfWork = unitOfWork;
        _workerRepository = workerRepository;
    }

    public async Task<GetWorkerGeneralReportResponseDto> Handle(GetWorkerGeneralReportQuery request, CancellationToken cancellationToken)
    {
        var worker = await _workerRepository.GetAsync(q => q.Id == request.Id) ?? throw new NotFoundException(nameof(Worker), request.Id);

        //var classSessions = await _unitOfWork.ClassSessionRepository.GetAllAsync(q => q.WorkerId == request.Id, tracking: false, "Class");


        var response = new GetWorkerGeneralReportResponseDto
        {
            EMail = worker.Email,
            Name = worker.Name,
            Surname = worker.Surname,
            CompletedClasses = new List<GetWorkerClassesForGeneralReportDto>(),
            UnCompletedClasses = new List<GetWorkerClassesForGeneralReportDto>()
        };



        //foreach (var item in classSessions)
        //{
        //    if (item.Status == ClassSessionStatus.Cancelled)
        //        response.CanceledHours += item.TotalHour;
        //    if (item.Status == ClassSessionStatus.Offline)
        //        response.CompletedHours += item.TotalHour;
        //    if (item.Status == ClassSessionStatus.Online)
        //        response.CompletedHours += item.TotalHour;


        //    response.TotalHours += item.TotalHour;

        //    bool hasClassInData = false;

        //    if (response.CompletedClasses.Count > 0)
        //        hasClassInData = response.CompletedClasses.Any(q => q.ClassId == item.ClassId);

        //    if (response.UnCompletedClasses.Count > 0)
        //        hasClassInData = response.UnCompletedClasses.Any(q => q.ClassId == item.ClassId);

        //    if (!hasClassInData)
        //    {

        //        var workerClass = new GetWorkerClassesForGeneralReportDto();
        //        workerClass.ClassId = item.ClassId;
        //        workerClass.ClassName = item.Class?.Name;
        //        workerClass.EndDate = item.Class?.EndDate;
        //        workerClass.StartDate = item.Class?.StartDate;

        //        if (item.Class?.EndDate > DateTime.Now && item.Class.EndDate != null)
        //        {
        //            response.UnCompletedClasses.Add(workerClass);
        //            response.TotalClasses++;
        //        }
        //        else
        //        {
        //            response.CompletedClasses.Add(workerClass);
        //            response.TotalClasses++;
        //        }
        //    }
        //}

        response.AttendancePercent = (100 * response.CompletedHours) / response.TotalHours;




        return response;
    }
}



