namespace Space.Application.Abstractions;

public interface IStudyRepository : IRepository<Study>
{
    IEnumerable<Study> GetAllAbsentStudies();
}
