using Space.Domain.Entities;
using System.Linq;

namespace Space.Application.Handlers.Queries;

public record GetAllClassQuery(
    ClassStatus Status,
    int? ProgramId,
    int? StudyCount,
    StudyCountStatus? StudyCountStatus,
    DateTime? StartDate,
    DateTime? EndDate,
    int? TeacherId,
    int? MentorId,
    int? StartAttendancePercentage,
    int? EndAttendancePercentage) : IRequest<IEnumerable<GetClassModuleWorkersResponse>>;
internal class GetAllClassHandler : IRequestHandler<GetAllClassQuery, IEnumerable<GetClassModuleWorkersResponse>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllClassHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassModuleWorkersResponse>> Handle(GetAllClassQuery request, CancellationToken cancellationToken)
    {
        IQueryable<Class> query = _spaceDbContext.Classes
            .Include(c => c.Studies)
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .Include(c => c.ClassSessions)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .ThenInclude(c => c.UserRoles)
            .ThenInclude(c => c.Role)
            .Include(c => c.Session)
            .AsQueryable();

        DateOnly now = DateOnly.FromDateTime(DateTime.Now);


        if (request.Status == ClassStatus.Close)
        {
            query = query.Where(c => now > c.EndDate);
        }
        else if (request.Status == ClassStatus.Active)
        {
            query = query.Where(c => now > c.StartDate && now < c.EndDate && c.ClassSessions.Count > 0);
        }
        else
        {
            query = query.Where(c => now < c.EndDate && c.ClassSessions.Count == 0);
        }
        if (request.StartDate is not null)
        {
            DateOnly startDateOnly = DateOnly.FromDateTime(request.StartDate.Value);
            query = query.Where(c => c.StartDate >= startDateOnly);
        }
        if (request.EndDate != null)
        {
            DateOnly endDateOnly = DateOnly.FromDateTime(request.EndDate.Value);

            query = query.Where(c => c.EndDate <= endDateOnly);
        }
        if (request.ProgramId != null)
        {
            query = query.Where(c => c.ProgramId == request.ProgramId);
        }
        if (request.StudyCount != null)
        {
            query = request.StudyCountStatus switch
            {
                StudyCountStatus.Equal => query.Where(c => c.Studies.Count == request.StudyCount),
                StudyCountStatus.Less => query.Where(c => c.Studies.Count <= request.StudyCount),
                StudyCountStatus.More => query.Where(c => c.Studies.Count >= request.StudyCount),
                _ => query.Where(c => c.Studies.Count == request.StudyCount),
            };
        }
        List<GetClassModuleWorkers> classes = await query.Select(cd => new GetClassModuleWorkers()
        {
            Id = cd.Id,
            TotalHour = cd.Program.TotalHours,
            CurrentHour = cd.ClassTimeSheets
                            .Where(c => c.Status != ClassSessionStatus.Cancelled &&
                                        c.Category != ClassSessionCategory.Lab && c.Category != ClassSessionCategory.Practice)
                            .Sum(c => c.TotalHours),
            ClassName = cd.Name,
            EndDate = cd.EndDate,
            ProgramId = cd.ProgramId,
            ProgramName = cd.Program.Name,
            SessionName = cd.Session.Name,
            StudyCount = cd.Studies.Count,
            StartDate = cd.StartDate,
            TotalModules = cd.Program.Modules.Count,
            ClassModulesWorkers = cd.ClassModulesWorkers
        }).ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<GetClassModuleWorkersResponse> classesResponse = classes.Select(cd => new GetClassModuleWorkersResponse()
        {
            Id = cd.Id,
            TotalHour = cd.TotalHour,
            ClassName = cd.ClassName,
            CurrentHour = cd.CurrentHour,
            EndDate = cd.EndDate,
            StudyCount = cd.StudyCount,
            ProgramId = cd.ProgramId,
            ProgramName = cd.ProgramName,
            SessionName = cd.SessionName,
            StartDate = cd.StartDate,
            TotalModules = cd.TotalModules,
            Workers = cd.ClassModulesWorkers.Select(cmw => new GetWorkerForClassDto()
            {
                Id = cmw.WorkerId,
                Email = cmw.Worker.Email,
                Name = cmw.Worker.Name,
                Surname = cmw.Worker.Surname,
                Role = cmw.Worker.UserRoles.FirstOrDefault(u => u.RoleId == cmw.RoleId)?.Role.Name,
                RoleId = cmw.Worker.UserRoles.FirstOrDefault(u => u.RoleId == cmw.RoleId)?.Role.Id
            }).Distinct(new GetModulesWorkerComparer())
        });
        if (request.TeacherId != null)
        {
            classesResponse = classesResponse.Where(c => c.Workers.Any(c => c.Id == request.TeacherId && c.Role == RoleEnum.muellim.ToString()));
        }
        if (request.MentorId != null)
        {
            classesResponse = classesResponse.Where(c => c.Workers.Any(c => c.Id == request.TeacherId && c.Role == RoleEnum.mentor.ToString()));
        }
        if (request.StartAttendancePercentage != null)
        {
            classesResponse = classesResponse.Where(c => c.TotalHour >= request.StartAttendancePercentage);
        }
        if (request.EndAttendancePercentage != null)
        {
            classesResponse = classesResponse.Where(c => c.TotalHour <= request.StartAttendancePercentage);
        }
        return classesResponse.OrderByDescending(c => c.CurrentHour);
    }
}

