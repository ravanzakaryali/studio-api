namespace Space.Application.Handlers;


public record GetAllStudentsQuery : IRequest<IEnumerable<GetAllStudentsResponseDto>>;
internal class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<GetAllStudentsResponseDto>>
{
    readonly ISpaceDbContext _spaceDbContext;

    public GetAllStudentsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }
    public async Task<IEnumerable<GetAllStudentsResponseDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        //Todo: Contact nullable
        List<Study> studies = await _spaceDbContext.Studies
            .Include(c => c.Student)
            .ThenInclude(c => c.Contact)
            .Include(c => c.Class)
            .Where(q => q.Class!.EndDate > DateOnly.FromDateTime(DateTime.Now))
            .ToListAsync(cancellationToken: cancellationToken);

        List<GetAllStudentsResponseDto> response = new();


        foreach (var item in studies)
        {
            if (item.Class == null) break;
            if (item.Student != null)
            {
                var hasStudentInData = response.FirstOrDefault(q => q.Id == item.Student.Id);

                if (hasStudentInData != null)
                {
                    var studentClass = new GetAllStudentsClassDto
                    {
                        Id = item.Class.Id,
                        Name = item.Class.Name
                    };

                    hasStudentInData.Classes.Add(studentClass);


                }
                else
                {
                    var model = new GetAllStudentsResponseDto
                    {
                        Classes = new List<GetAllStudentsClassDto>(),
                        Id = item.Student.Id,
                        EMail = item.Student.Email,
                        Name = item.Student.Contact?.Name,
                        Surname = item.Student.Contact?.Surname,
                        Phone = item.Student.Contact?.Phone
                    };

                    var studentClass = new GetAllStudentsClassDto
                    {
                        Id = item.Class.Id,
                        Name = item.Class.Name
                    };

                    model.Classes.Add(studentClass);


                    response.Add(model);
                }
            }
        }
        return response;
    }
}


