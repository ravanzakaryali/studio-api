using Microsoft.EntityFrameworkCore;
using Space.Domain.Enums;

namespace Space.Infrastructure.Persistence;

internal class StudyRespository : Repository<Study>, IStudyRepository
{
    readonly SpaceDbContext _dbContext;
    public StudyRespository(SpaceDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public IEnumerable<Study> GetAllAbsentStudies()
    {
        IQueryable<Study> result = _dbContext.Studies.Include("Student.Contact").Include("Attendances").Include("Class").Where(study => study.Attendances.Any(a => a.Status == StudentStatus.Absent) &&
                           study.Attendances.GroupBy(a => a.ClassSession.Date)
                                           .Any(g => g.Count() >= 3)).Select(c => c);
        return result;
    }
}
