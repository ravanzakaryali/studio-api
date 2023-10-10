using System;

namespace Space.Application.Handlers
{
    public record GetWorkerAttendanceByClassQuery(Guid Id) : IRequest<IEnumerable<GetWorkerAttendanceByClassDto>>;


    internal class GetWorkerAttendanceByClassQueryHandler : IRequestHandler<GetWorkerAttendanceByClassQuery, IEnumerable<GetWorkerAttendanceByClassDto>>
    {

        readonly IUnitOfWork _unitOfWork;

        public GetWorkerAttendanceByClassQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<GetWorkerAttendanceByClassDto>> Handle(GetWorkerAttendanceByClassQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.ClassSessionRepository.GetAllAsync(q => q.ClassId == request.Id && (q.Status == ClassSessionStatus.Online || q.Status == ClassSessionStatus.Offline));

            var response = new List<GetWorkerAttendanceByClassDto>();

            //var data2 = data.GroupBy(q => q.WorkerId).Select(q => new
            //{
            //    id = q.Key,
            //    total = q.Sum(item => item.TotalHour)
            //});


           
            //foreach (var item in data2)
            //{
            //    GetWorkerAttendanceByClassDto model = new GetWorkerAttendanceByClassDto();

            //    model.WorkerId = item.id;
            //    model.Hours = item.total;
            //    //code review  yapılmalı. yukarıda join ile worker alınabilir!
            //    var worker = await _unitOfWork.WorkerRepository.GetAsync(q => q.Id == item.id);
            //    model.FullName = worker.Name + " " + worker.Surname;
            //    string roles = "";

            //    if(worker.UserRoles != null)
            //    {
            //        if (worker.UserRoles.Count > 0)
            //        {
            //            foreach (var role in worker.UserRoles)
            //            {
            //                roles = roles + role.Role?.Name;
            //            }
            //        }
            //    }
                

            //    model.Role = roles;

            //    response.Add(model);

            //}

            return response;

        }
    }

}

