using Space.Domain.Entities;
using System.Globalization;

namespace Space.Application.Handlers;

public record GetAllAbsentStudentsQuery(Guid Id) : IRequest<IEnumerable<GetAllAbsentStudentResponseDto>>;

public class GetAllAbsentStudentsQueryHandler : IRequestHandler<GetAllAbsentStudentsQuery, IEnumerable<GetAllAbsentStudentResponseDto>>
{

    readonly ISpaceDbContext _spaceDbContext;

    public GetAllAbsentStudentsQueryHandler(
        ISpaceDbContext spaceDbContext)
    {
        _spaceDbContext = spaceDbContext;
    }

    public async Task<IEnumerable<GetAllAbsentStudentResponseDto>> Handle(GetAllAbsentStudentsQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _spaceDbContext.Classes
            .Where(c => c.Id == request.Id)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Attendances)
            .ThenInclude(c => c.ClassSession)
            .Include(c => c.Studies)
            .ThenInclude(c => c.Student)
            .ThenInclude(c => c.Contact)
            .FirstOrDefaultAsync() ??
                throw new NotFoundException(nameof(Class), request.Id);

        var response = new List<GetAllAbsentStudentResponseDto>();

        foreach (Study study in @class.Studies.Where(c => c.StudyType != StudyType.Completion))
        {
            var orderedAttendances = study.Attendances
                .Where(c => c.ClassSession.Category != ClassSessionCategory.Lab)
                .OrderBy(c => c.ClassSession.Date)
                .ToList();

            int consecutiveAbsentDays = 0;
            DateTime previousAttendanceDate = DateTime.MinValue;

            foreach (var attendance in orderedAttendances)
            {
                if (attendance.TotalAttendanceHours == 0)
                {
                    consecutiveAbsentDays++;

                    if (consecutiveAbsentDays == 1)
                    {
                        response.Add(new GetAllAbsentStudentResponseDto()
                        {
                            Id = study!.Id,
                            StudentId = study.StudentId,
                            Name = study!.Student!.Contact!.Name,
                            Surname = study!.Student!.Contact!.Surname,
                            Father = study?.Student?.Contact?.FatherName,
                            Class = new GetAllClassDto()
                            {
                                Name = @class.Name,
                                Id = @class.Id
                            },
                            AbsentCount = 1
                        });
                    }
                    else
                    {
                        response.Last().AbsentCount = consecutiveAbsentDays;
                    }
                }
                else
                {
                    consecutiveAbsentDays = 0;
                }

                previousAttendanceDate = attendance.ClassSession.Date;
            }
        }

        return response.Where(c => c.AbsentCount >= 3);
    }
}
