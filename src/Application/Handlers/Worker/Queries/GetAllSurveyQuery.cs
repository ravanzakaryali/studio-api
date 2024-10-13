using System;
using Space.Application.DTOs.Workers.Response;

namespace Space.Application.Handlers
{

    public record GetAllSurveyQuery : IRequest<IEnumerable<GetAllSurveyDto>>;

    internal class GetAllSurveyQueryHandler : IRequestHandler<GetAllSurveyQuery, IEnumerable<GetAllSurveyDto>>
    {

        readonly ISpaceDbContext _spaceDbContext;

        public GetAllSurveyQueryHandler(
            ISpaceDbContext spaceDbContext)
        {
            _spaceDbContext = spaceDbContext;
        }

        public async Task<IEnumerable<GetAllSurveyDto>> Handle(GetAllSurveyQuery request, CancellationToken cancellationToken)
        {

            //List<ClassModulesWorker> workersClasses = await _spaceDbContext.ClassModulesWorkers.Where(c => c.StartDate != null && c.EndDate != null).Include(m => m.Module).ToListAsync(cancellationToken: cancellationToken);

            List<Class> classes = await _spaceDbContext.Classes.Include(x => x.ClassModulesWorkers).ThenInclude(x => x.Module).Include(r => r.Room).Include(p => p.Program).ToListAsync(cancellationToken: cancellationToken);
            //List<Module> modules = await _spaceDbContext.Modules.DistinctBy(x=>x.Id).ToListAsync(cancellationToken: cancellationToken);
            List<SurveySheet> SurveySheets = await _spaceDbContext.SurveySheets.ToListAsync();
            // sessions

            var response = new List<GetAllSurveyDto>();

            foreach (var item in classes)
            {
                //if (item == null) break;
                //var model = new GetAllExamDto
                //{
                //    ClassName = item.Name,
                //    RoomName = item.Room.Name,

                //};

                var model = new GetAllSurveyDto()
                {
                    ClassName = item?.Name,
                    RoomName = item.Room?.Name,
                    ClassId = item.Id,
                    RoomId = item.Room?.Id,
                    IsOpen = true,
                    ProgramName = item.Program.Name
                };


                model.Modules = new List<GetAllSurveyModulesDto>();

                foreach (var test in item.ClassModulesWorkers.DistinctBy(x => x.ModuleId))
                {
                    //if (item == null) break;
                    var moduleDto = new GetAllSurveyModulesDto()
                    {
                        EndDate = test?.EndDate,
                        StartDate = test?.StartDate,
                        ModulName = test.Module?.Name,
                        ModuleId = test.Module?.Id,
                        IsSurvey = false

                    };


                    if (SurveySheets.Any(sheet => sheet.ClassId == model.ClassId && sheet.ModuleId == test.Module?.Id))
                    {
                        moduleDto.IsSurvey = true;
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
}

