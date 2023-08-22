namespace Space.Application.Abstractions;


public interface IStudentRepository : IRepository<Student>
{
    Task GenerateEmailAsync(Guid id);
}

