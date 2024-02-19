namespace Space.Application.Handlers;

public record GetClassWorkersModulesQuery(int Id, int? SessionId) : IRequest<GetAllClassModuleDto>;

internal class GetClassWorkersModulesQueryHandler : IRequestHandler<GetClassWorkersModulesQuery, GetAllClassModuleDto>
{
    readonly IMapper _mapper;
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;
    readonly ICurrentUserService _currentUserService;

    public GetClassWorkersModulesQueryHandler(
        IMapper mapper,
        ICurrentUserService currentUserService,
        ISpaceDbContext spaceDbContext,
        IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _currentUserService = currentUserService;
        _spaceDbContext = spaceDbContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<GetAllClassModuleDto> Handle(GetClassWorkersModulesQuery request, CancellationToken cancellationToken)
    {
        Class? @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Program)
            .Include(c => c.ClassTimeSheets)
            .Include(c => c.ClassSessions)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
                throw new NotFoundException(nameof(Class), request.Id);

        int sessionId = request.SessionId ?? @class.SessionId;
        Session? session = await _spaceDbContext.Sessions
            .Where(c => c.Id == sessionId)
            .Include(c => c.Details)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken) ??
            throw new NotFoundException(nameof(Session), sessionId);

        //modules
        List<Module> modules = await _spaceDbContext.Modules
            .Include(c => c.SubModules)
            .Where(c => c.ProgramId == @class.ProgramId && c.TopModuleId == null)
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        //class modules workers
        List<ClassModulesWorker> classModulesWorkers = await _spaceDbContext.ClassModulesWorkers
            .Where(c => c.ClassId == @class.Id)
            .Include(c => c.Role)
            .Include(c => c.Worker)
            .ToListAsync(cancellationToken: cancellationToken);





        //if (@class.Program.Modules.Any()) throw new NotFoundException("The class has no modules");

        List<GetClassModuleResponseDto> modulesReponse = modules.Select(m => new GetClassModuleResponseDto()
        {
            Id = m.Id,
            Name = m.Name,
            Hours = m.Hours,
            Version = m.Version,
            SubModules = m.SubModules?.Select(sub => new SubModuleDtoWithWorker()
            {
                Name = sub.Name,
                Hours = sub.Hours,
                Id = sub.Id,
                TopModuleId = sub.TopModuleId,
                Version = sub.Version,
                Workers = _mapper.Map<ICollection<GetWorkerForClassDto>>(classModulesWorkers.Where(cmw => cmw.ModuleId == sub.Id))
            }).ToList(),
            Workers = _mapper.Map<ICollection<GetWorkerForClassDto>>(classModulesWorkers.Where(cmw => m.SubModules!.Any(s => s.Id == cmw.ModuleId)).Distinct(new GetWorkerForClassDtoComparer()))
        }).ToList();

        List<ClassDateHourDto> classDateTimes = new();
        DateOnly startDateTime = @class.StartDate;



        int count = 0;
        List<DateOnly> holidayDates = await _unitOfWork.HolidayService.GetDatesAsync();
        int totalHour = @class.Program.TotalHours;

        if (@class.ClassSessions == null || !@class.ClassSessions.Any())
        {
            while (totalHour > 0)
            {
                foreach (SessionDetail? sessionItem in session.Details.OrderBy(c => c.DayOfWeek))
                {
                    var daysToAdd = ((int)sessionItem.DayOfWeek - (int)startDateTime.DayOfWeek + 7) % 7;
                    int numSelectedDays = session.Details.Count;

                    int hour = (sessionItem.EndTime - sessionItem.StartTime).Hours;


                    DateOnly dateTime = startDateTime.AddDays(count * 7 + daysToAdd);

                    if (holidayDates.Contains(dateTime))
                    {
                        continue;
                    }

                    if (hour != 0)
                    {
                        classDateTimes.Add(new ClassDateHourDto()
                        {
                            DateTime = dateTime,
                            Hour = hour,
                        });
                        if (sessionItem.Category != ClassSessionCategory.Lab)
                            totalHour -= hour;
                        if (totalHour <= 0)
                            break;

                    }
                }
                count++;
            }
        }
        else
        {
            classDateTimes = @class.ClassSessions.Select(c => new ClassDateHourDto()
            {
                DateTime = c.Date,
                Hour = c.TotalHours,
            }).ToList();

        }



        classDateTimes = classDateTimes.OrderBy(c => c.DateTime).ToList();

        modulesReponse = modulesReponse.OrderBy(m => int.Parse(m.Version!)).ToList();

        foreach (var item in modulesReponse)
        {
            item.SubModules = item.SubModules?
               .OrderBy(m => Version.TryParse(m.Version, out var parsedVersion) ? parsedVersion : null).ToList();
        }


        // foreach (GetClassModuleResponseDto item in modulesReponse)
        // {
        //     if (classModulesWorkers.Where(c => c.ModuleId == item.Id).FirstOrDefault() != null)
        //     {
        //         item.StartDate = classModulesWorkers.Where(c => c.ModuleId == item.Id).First().StartDate;
        //         item.EndDate = classModulesWorkers.Where(c => c.ModuleId == item.Id).First().EndDate;

        //     }
        //     if (item.SubModules != null)
        //     {
        //         foreach (SubModuleDtoWithWorker subModules in item.SubModules)
        //         {
        //             if (classModulesWorkers.Where(c => c.ModuleId == subModules.Id).FirstOrDefault() != null)
        //             {

        //                 subModules.StartDate = classModulesWorkers.Where(c => c.ModuleId == subModules.Id).First().StartDate;
        //                 subModules.EndDate = classModulesWorkers.Where(c => c.ModuleId == subModules.Id).First().EndDate;
        //             }
        //         }
        //     }

        // }
        if (modulesReponse[0].StartDate == null)
        {

            modulesReponse[0].StartDate = classDateTimes.First().DateTime;
        }
        if (modulesReponse.Last().EndDate == null)
        {
            modulesReponse.Last().EndDate = classDateTimes.Last().DateTime;
        }

        //modulların sayı qədər dövr etsin





        for (int i = 0; i < modulesReponse.Count; i++)
        {
            //modullun sub modullu varsa daxil olsun
            if (modulesReponse[i].SubModules != null)
            {
                //modulların sub modulda dövr etsin
                for (int j = 0; j < modulesReponse[i].SubModules!.Count; j++)
                {
                    int subModuleSum = 0;

                    //moddulun ilk submodulu onun start ilə bərabər olsun. 

                    if (i == 0)
                    {
                        // ilk dÖvrdə ilk modulun start date olduğu üçün onu götürsün.
                        if (modulesReponse[i].SubModules![0].StartDate == null)
                        {
                            modulesReponse[i].SubModules![0].StartDate = modulesReponse[0].StartDate;
                        }
                    }
                    else
                    {
                        //digər dövlərdə bir əvvəli modulun ən sonuncusnun end date götürsün
                        if (modulesReponse[i].SubModules![j].StartDate == null)
                        {
                            modulesReponse[i].SubModules![j].StartDate = modulesReponse[i - 1].SubModules![^1].EndDate;
                        }
                    }
                    if (j != 0)
                    {

                        foreach (ClassDateHourDto? classDateHour in
                                                      classDateTimes.Where(c => c.DateTime > modulesReponse[i].SubModules![j - 1].StartDate))
                        {
                            //classın sessionlarının saatlarını hesablasın
                            subModuleSum += classDateHour.Hour;

                            //əgər toplam saat subModules saatından böyük olarsa o zaman daxil olsun
                            if (subModuleSum >= modulesReponse[i].SubModules![j].Hours)
                            {
                                if (modulesReponse[i].SubModules![j].EndDate == null)
                                {
                                    modulesReponse[i].SubModules![j].EndDate = classDateHour.DateTime;
                                }
                                if (j > 0)
                                {
                                    modulesReponse[i].SubModules![j].StartDate = modulesReponse[i].SubModules![j - 1].EndDate;
                                }
                                subModuleSum = 0;
                                break;
                            }
                        }
                    }
                    else
                    {
                        foreach (ClassDateHourDto? classDateHour in
                                                    classDateTimes.Where(c => c.DateTime >= modulesReponse[i].SubModules![j].StartDate))
                        {
                            //classın sessionlarının saatlarını  hesablasın
                            subModuleSum += classDateHour.Hour;

                            //əgər toplam saat subModules saatından böyük olarsa o zaman daxil olsun
                            if (subModuleSum >= modulesReponse[i].SubModules![j].Hours)
                            {
                                if (modulesReponse[i].SubModules![j].EndDate == null)
                                {
                                    modulesReponse[i].SubModules![j].EndDate = classDateHour.DateTime;
                                }
                                subModuleSum = 0;
                                break;
                            }
                        }
                    }
                }
            }
        }



        if (modulesReponse[0].StartDate == null)
        {
            modulesReponse[0].StartDate = classDateTimes.First().DateTime;
        }


        foreach (GetClassModuleResponseDto moduleItem in modulesReponse)
        {
            moduleItem.StartDate = moduleItem.SubModules?.Min(c => c.StartDate);
            moduleItem.EndDate = moduleItem.SubModules?.Max(c => c.EndDate);
        }

        modulesReponse.Last().EndDate = classDateTimes.Last().DateTime;
        if (modulesReponse.Last().SubModules != null)
        {
           modulesReponse.Last().SubModules.Last().EndDate = classDateTimes.Last().DateTime;
        }


        //Extra modules 
        List<ExtraModule> extraModules = await _spaceDbContext.ExtraModules
            .Where(c => c.ProgramId == @class.ProgramId)
            .ToListAsync(cancellationToken: cancellationToken);

        //class extra modules workers
        List<ClassExtraModulesWorkers> classExtraModulesWorkers = await _spaceDbContext.ClassExtraModulesWorkers
            .Where(c => c.ClassId == @class.Id)
            .Include(c => c.Role)
            .Include(c => c.Worker)
            .Include(c => c.ExtraModule)
            .ToListAsync(cancellationToken: cancellationToken);

        List<GetClassExtraModuleResponseDto> extraModulesResponse = classExtraModulesWorkers
            .DistinctBy(c => c.ExtraModuleId)
            .Select(m => new GetClassExtraModuleResponseDto()
            {
                ExtraModuleId = m.ExtraModuleId,
                Name = m.ExtraModule.Name,
                Hours = m.ExtraModule.Hours,
                Version = m.ExtraModule.Version,
                StartDate = m.StartDate,
                EndDate = m.EndDate,
                Workers = classExtraModulesWorkers.Where(c => c.ExtraModuleId == m.ExtraModuleId && c.ClassId == m.ClassId).Select(ex => new GetWorkerForClassDto()
                {
                    Id = ex.WorkerId,
                    Name = ex.Worker.Name,
                    Surname = ex.Worker.Surname,
                    Role = ex.Role.Name,
                    Email = ex.Worker.Email,
                    RoleId = ex.RoleId,
                }).ToList(),
            }).ToList();

        foreach (GetClassModuleResponseDto moduleItem in modulesReponse)
        {
            moduleItem.StartDate = moduleItem.SubModules?.Min(c => c.StartDate);
            moduleItem.EndDate = moduleItem.SubModules?.Max(c => c.EndDate);
        }

        return new GetAllClassModuleDto()
        {
            Modules = modulesReponse,
            ExtraModules = extraModulesResponse,
        };
    }
}

