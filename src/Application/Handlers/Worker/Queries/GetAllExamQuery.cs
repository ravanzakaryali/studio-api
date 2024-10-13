using System;
using System.Collections.Generic;
using Space.Application.DTOs;
using Space.Application.DTOs.Workers.Response;

namespace Space.Application.Handlers;


public record GetAllExamQuery : IRequest<IEnumerable<GetAllExamDto>>;

internal class GetAllExamQueryHandler : IRequestHandler<GetAllExamQuery, IEnumerable<GetAllExamDto>>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetAllExamQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllExamDto>> Handle(GetAllExamQuery request, CancellationToken cancellationToken)
    {

        //List<ClassModulesWorker> workersClasses = await _spaceDbContext.ClassModulesWorkers.Where(c => c.StartDate != null && c.EndDate != null).Include(m => m.Module).ToListAsync(cancellationToken: cancellationToken);

        List<Class> classes = await _spaceDbContext.Classes.Include(x=>x.ClassModulesWorkers).ThenInclude(x=>x.Module).Include(r => r.Room).Include(p=>p.Program).Where(p=>p.Program.Name == "Proqramlaşdırma").ToListAsync(cancellationToken: cancellationToken);
        //List<Module> modules = await _spaceDbContext.Modules.DistinctBy(x=>x.Id).ToListAsync(cancellationToken: cancellationToken);
        List<ExamSheet> examSheets = await _spaceDbContext.ExamSheets.ToListAsync();
        // sessions

        var response = new List<GetAllExamDto>();

        foreach (var item in classes)
        {
            //if (item == null) break;
            //var model = new GetAllExamDto
            //{
            //    ClassName = item.Name,
            //    RoomName = item.Room.Name,

            //};

            var model = new GetAllExamDto()
            {
                ClassName = item?.Name,
                RoomName = item.Room?.Name,
                ClassId = item.Id,
                RoomId = item.Room?.Id,
                IsOpen = true,
                ProgramName = item.Program.Name
            };


            model.Modules = new List<GetAllExamModulesDto>();

            foreach (var test in item.ClassModulesWorkers.DistinctBy(x=>x.ModuleId))
            {
                //if (item == null) break;
                var moduleDto = new GetAllExamModulesDto()
                {
                    EndDate = test?.EndDate,
                    StartDate = test?.StartDate,
                    ModulName = test.Module?.Name,
                    ModuleId = test.Module?.Id,
                    IsExam = false

                };
                

                if (examSheets.Any(sheet => sheet.ClassId == model.ClassId && sheet.ModuleId == test.Module?.Id))
                {
                    moduleDto.IsExam = true;
                }
                model.Modules.Add(moduleDto);
            }



            response.Add(model);


          //List<ClassModulesWorker> classModules = workersClasses.Where(c => c.ClassId == item.Id && c.EndDate != null && c.StartDate != null ).Distinct().ToList();

            //model.Modules.AddRange(classModules.Select(classModule => new GetAllExamModulesDto()
            //{
            //    ModuleId = classModule.Module.Id,
            //    ModulName = classModule.Module.Name,
            //    StartDate = (DateOnly)classModule.StartDate,
            //    EndDate = (DateOnly)classModule.EndDate,
            //    IsExam = classModule.Module.IsExam
            //}));


            

           
        }

        return response;

    }
}