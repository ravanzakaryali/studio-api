namespace Space.Application.Abstractions;

public interface IStudyRepository : IRepository<Study>
{
    Task<IEnumerable<Study>> GetAllAbsentStudies();
}
