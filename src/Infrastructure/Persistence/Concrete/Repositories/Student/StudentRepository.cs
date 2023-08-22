using Space.Application.Exceptions;
using Space.Application.Extensions;

namespace Space.Infrastructure.Persistence;
internal class StudentRepository : Repository<Student>, IStudentRepository
{

    readonly SpaceDbContext _dbContext;
    public StudentRepository(SpaceDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task GenerateEmailAsync(Guid id)
    {
        Student? student = await _dbContext.Students.Include(c => c.Contact).FirstOrDefaultAsync(s => s.Id == id) ??
            throw new NotFoundException(nameof(Student), id);

        if (student.Contact == null) throw new NotFoundException(nameof(Contact));
        student.Contact.Email = student.Contact.Name.CharacterRegulatory() +
                                student.Contact.FatherName.CharacterRegulatory()[0] +
                                student.Contact.Surname.CharacterRegulatory()[0] + 
                                "@code.edu.az";
        await _dbContext.SaveChangesAsync();
    }
}

