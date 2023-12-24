
namespace Space.Application.Handlers;

public class StudentsOfClassesExcelExportCommand : IRequest<FileContentResponseDto>
{
    public ClassStatus ClassStatus { get; set; }
    public List<int>? ClassIds { get; set; }
    public List<int>? ProgramIds { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
internal class StudentsOfClassesExcelExportHandler : IRequestHandler<StudentsOfClassesExcelExportCommand, FileContentResponseDto>
{
    readonly ISpaceDbContext _spaceDbContext;
    readonly IHttpContextAccessor _contextAccessor;

    public StudentsOfClassesExcelExportHandler(ISpaceDbContext spaceDbContext, IHttpContextAccessor contextAccessor)
    {
        _spaceDbContext = spaceDbContext;
        _contextAccessor = contextAccessor;
    }

    public async Task<FileContentResponseDto> Handle(StudentsOfClassesExcelExportCommand request, CancellationToken cancellationToken)
    {
        //IQueryable<Class> query = _spaceDbContext.Classes
        //    .Include(c => c.ClassTimeSheets)
        //    .ThenInclude(c => c.Attendances)
        //    .ThenInclude(c => c.Student)
        //    .ThenInclude(c => c.Student)
        //    .ThenInclude(c => c!.Contact)
        //    .AsQueryable();
        //query = request.ClassStatus switch
        //{
        //    StudyCountStatus.Equal => query.Where(c => c.Studies.Count == request.StudyCount),
        //    StudyCountStatus.Less => query.Where(c => c.Studies.Count <= request.StudyCount),
        //    StudyCountStatus.More => query.Where(c => c.Studies.Count >= request.StudyCount),
        //    _ => query.Where(c => c.Studies.Count == request.StudyCount),
        //};
        throw new Exception();
    }
}
