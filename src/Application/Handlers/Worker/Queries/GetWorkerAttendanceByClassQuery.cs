using System;

namespace Space.Application.Handlers
{
    public record GetWorkerAttendanceByClassQuery(Guid Id) : IRequest<IEnumerable<GetWorkerAttendanceByClassDto>>;


    internal class GetWorkerAttendanceByClassQueryHandler : IRequestHandler<GetWorkerAttendanceByClassQuery, IEnumerable<GetWorkerAttendanceByClassDto>>
    {

        readonly ISpaceDbContext _spaceDbContext;
        public GetWorkerAttendanceByClassQueryHandler(
            ISpaceDbContext spaceDbContext)
        {
            _spaceDbContext = spaceDbContext;
        }

        //Todo: İstifadə olunmur
        public async Task<IEnumerable<GetWorkerAttendanceByClassDto>> Handle(GetWorkerAttendanceByClassQuery request, CancellationToken cancellationToken)
        {

            //List<ClassTimeSheet> data = await _spaceDbContext.ClassSessions
            //    .Where(q => q.ClassId == request.Id && (q.Status == ClassSessionStatus.Online || q.Status == ClassSessionStatus.Offline))
            //    .ToListAsync();

            List<GetWorkerAttendanceByClassDto> response = new();

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

