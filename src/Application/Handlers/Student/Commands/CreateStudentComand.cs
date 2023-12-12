
namespace Space.Application.Handlers;

public class CreateStudentComand : IRequest
{
    public int ClassId { get; set; }
    public IEnumerable<CreateStudentsDto> Students { get; set; } = null!;
}
internal class CreateStudentHandler : IRequestHandler<CreateStudentComand>
{
    readonly ISpaceDbContext _spaceDbContext;

    public CreateStudentHandler(ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task Handle(CreateStudentComand request, CancellationToken cancellationToken)
    {
        //Todo: Contact is null
        Class @class = await _spaceDbContext.Classes
            .Include(c => c.Studies)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c!.Contact).FirstOrDefaultAsync(c => c.Id == request.ClassId, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Class), request.ClassId);

        @class.Studies = new List<Study>();

        foreach (CreateStudentsDto student in request.Students)
        {
            @class.Studies.Add(new Study()
            {
                ClassId = @class.Id,
                Status = "-",
                StudyType = StudyType.Study,
                Student = new Student()
                {
                    Contact = new Contact()
                    {
                        Name = student.Name,
                        Surname = student.Surname,
                        FatherName = student.FatherName,
                    }
                }
            });
        }
        await _spaceDbContext.SaveChangesAsync(cancellationToken);
    }
}
