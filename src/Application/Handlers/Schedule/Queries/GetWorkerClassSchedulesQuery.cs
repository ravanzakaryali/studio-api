using System;

namespace Space.Application.Handlers;

public record GetWorkerClassSchedulesQuery() : IRequest<IEnumerable<GetWorkersSchedulesResponseDto>>;



internal class GetWorkerClassSchedulesQueryHandler : IRequestHandler<GetWorkerClassSchedulesQuery, IEnumerable<GetWorkersSchedulesResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetWorkerClassSchedulesQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetWorkersSchedulesResponseDto>> Handle(GetWorkerClassSchedulesQuery request, CancellationToken cancellationToken)
    {

        List<Worker> workers = await _spaceDbContext.Workers.ToListAsync();

        //var classSessions = await _spaceDbContext.ClassSessions
        //    .Include(c => c.AttendancesWorkers)
        //    .ToListAsync();
        List<Class> classes = await _spaceDbContext.Classes.ToListAsync();


        var response = new List<GetWorkersSchedulesResponseDto>();
        foreach (var item in workers)
        {
            GetWorkersSchedulesResponseDto model = new()
            {
                WorkerName = item.Name + " " + item.Surname
            };

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