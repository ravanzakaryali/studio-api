﻿using Space.Application.Abstractions;

namespace Space.Application.Handlers;

public class CreateClassModuleSessionCommand : IRequest
{
    public int ClassId { get; set; }
    public CreateClassModuleSessionRequestDto CreateClassModuleSessionDto { get; set; } = null!;
}
internal class CreateClassModuleSessionHandler : IRequestHandler<CreateClassModuleSessionCommand>
{
    private readonly IMediator _mediator;
    private readonly ISpaceDbContext _spaceDbContext;
    readonly IUnitOfWork _unitOfWork;
    public CreateClassModuleSessionHandler(
        IMediator mediator,
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    //Todo: Request module change - High
    public async Task Handle(CreateClassModuleSessionCommand request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
           .Include(c => c.Session)
           .ThenInclude(c => c.Details)
           .Include(c => c.Program)
           .ThenInclude(c => c.Modules)
           .ThenInclude(c => c.SubModules)
           .Where(c => c.Id == request.ClassId)
           .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
               throw new NotFoundException(nameof(Class), request.ClassId);
        //Create Modules


        IEnumerable<int> moduleIds = request.CreateClassModuleSessionDto.Modules.Select(c => c.ModuleId);
        IEnumerable<Module> modules = await _spaceDbContext.Modules
            .Where(c => moduleIds.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        IEnumerable<int> existingModuleIds = modules.Select(m => m.Id);
        IEnumerable<int> nonExistingModuleIds = moduleIds.Except(existingModuleIds);
        if (nonExistingModuleIds.Any())
            throw new NotFoundException(nameof(Module), $"{string.Join(",", nonExistingModuleIds)}");

        //if(modules.Any(m=>m.TopModuleId == null))
        //{
        //    throw new Exception("When adding a module to a class, the parent module ID should not.");
        //}

        foreach (Module module in modules)
        {
            if (module.TopModuleId == null)
            {
                request.CreateClassModuleSessionDto.Modules.ToList().Remove(request.CreateClassModuleSessionDto.Modules.First(rm => rm.ModuleId == module.Id));
            }
        }


        IEnumerable<int> workerIds = request.CreateClassModuleSessionDto.Modules.Where(c => c.WorkerId != null).Select(c => c.WorkerId!.Value);
        //if (@class.Program.Modules.Any(c => moduleIds.Contains(c.Id))) throw new NotFoundException("Modules not found in modules of class program");
        IEnumerable<Worker> workers = await _spaceDbContext.Workers
            .Where(c => workerIds.Contains(c.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        IEnumerable<int> existingWorkerIds = workers.Select(w => w.Id);
        IEnumerable<int> nonExistingWorkerIds = workerIds.Except(existingWorkerIds);
        if (nonExistingWorkerIds.Any())
            throw new NotFoundException(nameof(Worker), $"{string.Join(",", nonExistingWorkerIds)}");

        IEnumerable<int> roleIds = request.CreateClassModuleSessionDto.Modules.Where(c => c.RoleId != null).Select(c => c.RoleId!.Value);
        IEnumerable<Role> roles = await _spaceDbContext.Roles.Where(c => roleIds.Contains(c.Id)).ToListAsync(cancellationToken: cancellationToken);
        IEnumerable<int> nonExistingRoleIds = roles.Select(w => w.Id);
        if (!nonExistingRoleIds.Any())
            throw new NotFoundException(nameof(Role), $"{string.Join(",", nonExistingRoleIds)}");

        IEnumerable<ClassModulesWorker> classModulesWorker = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == request.ClassId)
            .ToListAsync(cancellationToken: cancellationToken);
        _spaceDbContext.ClassModulesWorkers.RemoveRange(classModulesWorker);

        IEnumerable<ClassModulesWorker> classModuleWorkers = request.CreateClassModuleSessionDto.Modules.Where(c => c.WorkerId != null).Select(c => new ClassModulesWorker()
        {
            WorkerId = c.WorkerId!.Value,
            StartDate = c.StartDate,
            EndDate = c.EndDate,
            ModuleId = c.ModuleId,
            RoleId = c.RoleId,
            ClassId = @class.Id
        });
        await _spaceDbContext.ClassModulesWorkers.AddRangeAsync(classModuleWorkers, cancellationToken);

        _spaceDbContext.ClassSessions.RemoveRange(await _spaceDbContext.ClassSessions.Where(cr => cr.ClassId == @class.Id).ToListAsync(cancellationToken: cancellationToken));

        Session session = await _spaceDbContext.Sessions
            .Include(c => c.Details)
            .Where(c => c.Id == request.CreateClassModuleSessionDto.SessionId)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Session), request.CreateClassModuleSessionDto.SessionId);

        if (@class.RoomId == null)
            throw new Exception("Class room null");

        List<CreateClassSessionDto> sessions = session.Details
            .Select(c => new CreateClassSessionDto()
            {
                Category = c.Category,
                DayOfWeek = c.DayOfWeek,
                End = c.EndTime,
                Start = c.StartTime
            }).ToList();

        List<DayOfWeek> selectedDays = sessions.Select(c => c.DayOfWeek).ToList();

        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();


        List<ClassSession> classSessions = _unitOfWork.ClassSessionService.GenerateSessions(
                                                                                       @class.Program.TotalHours,
                                                                                       sessions,
                                                                                       @class.StartDate,
                                                                                       holidayDates,
                                                                                       @class.Id,
                                                                                       @class.RoomId.Value);

        @class.EndDate = classSessions.Max(c => c.Date);

        DateOnly maxModuleDate = request.CreateClassModuleSessionDto.Modules.Max(c => c.EndDate);
        DateOnly minModuleDate = request.CreateClassModuleSessionDto.Modules.Min(c => c.StartDate);

        @class.StartDate = minModuleDate;
        @class.EndDate = maxModuleDate;

        await _spaceDbContext.ClassSessions.AddRangeAsync(classSessions, cancellationToken);
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
