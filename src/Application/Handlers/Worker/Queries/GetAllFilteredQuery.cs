using System;
using System.Collections.Generic;

namespace Space.Application.Handlers;


public record GetAllFilteredQuery : IRequest<IEnumerable<GetFilteredDataDto>>;

internal class GetAllFilteredQueryHandler : IRequestHandler<GetAllFilteredQuery, IEnumerable<GetFilteredDataDto>>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetAllFilteredQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetFilteredDataDto>> Handle(GetAllFilteredQuery request, CancellationToken cancellationToken)
    {

        List<ClassModulesWorker> workersClasses = await _spaceDbContext.ClassModulesWorkers
                                .Where(c => c.StartDate != null && c.EndDate != null)
                                .Include(c => c.Class)
                                    .ThenInclude(p => p.Program)
                                .Include(c => c.Class)
                                    .ThenInclude(c => c.Room)
                                .Include(m => m.Module)
                                    .ThenInclude(m => m.TopModule)
                                    .ThenInclude(c => c!.SubModules)
                                .ToListAsync(cancellationToken: cancellationToken);

        List<Worker> workers = await _spaceDbContext.Workers
                .Include(c => c.UserRoles)
                .ThenInclude(c => c.Role)
                .ToListAsync(cancellationToken: cancellationToken);

        List<Session> sessions = await _spaceDbContext.Sessions
                .ToListAsync(cancellationToken: cancellationToken);

        // sessions
        var response = new List<GetFilteredDataDto>();

        foreach (var item in workers)
        {
            if (item == null) break;
            var model = new GetFilteredDataDto
            {
                ContractType = item.ContractType,
                EMail = item.Email,
                Name = item.Name,
                Roles = item.UserRoles.Select(c => c.Role.Name).ToList(),
                Surname = item.Surname,
                Id = item.Id
            };

            // workers -> sessions -> classes

            //IEnumerable<ClassModulesWorker> data = workersClasses.Where(q => q.WorkerId == item.Id);
            List<GetFilteredSessionDto> responsSessions = new();
            // sessions dto

            List<GetFilteredSessionDto> sessionDtos = new();
            foreach (var sesson in sessions)
            {
                var addSessions = new GetFilteredSessionDto()
                {
                    SessionId = sesson.Id,
                    SessionName = sesson.Name
                };


                List<ClassModulesWorker> classModules = workersClasses.Where(c => c.Class.SessionId == sesson.Id && c.WorkerId == item.Id && c.EndDate != null && c.StartDate != null).ToList();

                addSessions.GetFilteredDataClasses.AddRange(classModules.Select(classModule => new GetFilteredDataClassDto()
                {
                    ClassId = classModule.ClassId,
                    ModulName = classModule.Module.TopModule?.Name,
                    Module = new ModuleDto()
                    {
                        Name = classModule.Module.Name,
                        Version = classModule.Module.Version!,
                        Id = classModule.Module.Id,
                        Hours = classModule.Module.Hours,
                        SubModules = classModule.Module.SubModules?.Select(c => new CreateSubModuleDto()
                        {
                            Id = c.Id,
                            Name = c.Name,
                            Version = c.Version!,
                            Hours = c.Hours
                        }).ToList()
                    },
                    RoomId = classModule.Class.RoomId,
                    RoomName = classModule.Class.Room?.Name,
                    ClassName = classModule.Class.Name,
                    StartDate = (DateOnly)classModule.StartDate,
                    EndDate = (DateOnly)classModule.EndDate,
                    ProgramName = classModule.Class.Program.Name,
                    ProgramId = classModule.Class.ProgramId
                }));
                sessionDtos.Add(addSessions);
            }


            model.Sessions = sessionDtos.Where(c => c.GetFilteredDataClasses.Count != 0).ToList();

            response.Add(model);

        }


        return response;
    }

}
