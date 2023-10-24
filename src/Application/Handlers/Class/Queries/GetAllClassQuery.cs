using Serilog;
using Space.Application.DTOs;
using Space.Domain.Entities;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Space.Application.Handlers.Queries;

public record GetAllClassQuery : IRequest<IEnumerable<GetClassModuleWorkers>>;
internal class GetAllClassQueryHandler : IRequestHandler<GetAllClassQuery, IEnumerable<GetClassModuleWorkers>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly ISpaceDbContext _spaceDbContext;
    readonly IMapper _mapper;

    public GetAllClassQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ISpaceDbContext spaceDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetClassModuleWorkers>> Handle(GetAllClassQuery request, CancellationToken cancellationToken)
    {
        //IEnumerable<Class> classesDb = await _unitOfWork.ClassRepository.GetAllAsync(predicate: null, tracking: false, "Program.Modules", "ClassModulesWorkers.Worker.UserRoles.Role", "Session");

        List<Class> classes = await _spaceDbContext.Classes
            .Include(c => c.Program)
            .ThenInclude(c => c.Modules)
            .Include(c => c.ClassModulesWorkers)
            .ThenInclude(c => c.Worker)
            .ThenInclude(c => c.UserRoles)
            .ThenInclude(c => c.Role)
            .Include(c => c.Session)
            .ToListAsync(cancellationToken: cancellationToken);

        return classes.Select(cd => new GetClassModuleWorkers()
        {
            Id = cd.Id,
            ClassName = cd.Name,
            EndDate = cd.EndDate,
            IsNew = cd.IsNew,
            ProgramId = cd.ProgramId,
            ProgramName = cd.Program.Name,
            SessionName = cd.Session.Name,
            StartDate = cd.StartDate,
            TotalModules = cd.Program.Modules.Count,
            VitrinDate = cd.StartDate,
            Workers = cd.ClassModulesWorkers.Select(cmw => new GetWorkerForClassDto()
            {
                Id = cmw.Worker.Id,
                Email = cmw.Worker.Email,
                Name = cmw.Worker.Name,
                Surname = cmw.Worker.Surname,
                Role = cmw.Worker.UserRoles.FirstOrDefault(u => u.RoleId == cmw.RoleId)?.Role.Name,
                RoleId = cmw.Worker.UserRoles.FirstOrDefault(u => u.RoleId == cmw.RoleId)?.Role.Id
            }).Distinct(new GetModulesWorkerComparer())
        });
    }
    class GetModulesWorkerComparer : IEqualityComparer<GetWorkerForClassDto>
    {
        public bool Equals(GetWorkerForClassDto x, GetWorkerForClassDto y)
        {
            return x.Id == y.Id || x.RoleId == y.RoleId;
        }

        public int GetHashCode(GetWorkerForClassDto obj)
        {
            return obj.Id.GetHashCode() ^ obj.RoleId.GetHashCode();
        }

    }
}

