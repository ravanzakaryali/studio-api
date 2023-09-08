using Space.Domain.Entities;

namespace Space.Application.Handlers;

public record GetAllAbsentStudentsQuery(Guid Id) : IRequest<IEnumerable<GetAllAbsentStudentResponseDto>>;

public class GetAllAbsentStudentsQueryHandler : IRequestHandler<GetAllAbsentStudentsQuery, IEnumerable<GetAllAbsentStudentResponseDto>>
{

    readonly IUnitOfWork _unitOfWork;

    public GetAllAbsentStudentsQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GetAllAbsentStudentResponseDto>> Handle(GetAllAbsentStudentsQuery request, CancellationToken cancellationToken)
    {
        Class @class = await _unitOfWork.ClassRepository.GetAsync(request.Id, tracking: false, "Studies.Attendances.ClassSession", "Studies.Student.Contact")
    ?? throw new NotFoundException(nameof(Class), request.Id);

        var response = new List<GetAllAbsentStudentResponseDto>();

        foreach (Study study in @class.Studies.Where(c => c.StudyType != StudyType.Completion))
        {
            var orderedAttendances = study.Attendances
                .Where(c => c.TotalAttendanceHours == 0 && c.ClassSession.Category != ClassSessionCategory.Lab)
                .OrderBy(c => c.ClassSession.Date)
                .ToList();

            int consecutiveAbsentDays = 0;
            DateTime previousAttendanceDate = DateTime.MinValue;

            foreach (var attendance in orderedAttendances)
            {
                if ((attendance.ClassSession.Date - previousAttendanceDate).TotalDays > 1)
                {
                    int daysBetween = (int)(attendance.ClassSession.Date - previousAttendanceDate).TotalDays - 1;

                    consecutiveAbsentDays += daysBetween;
                }

                consecutiveAbsentDays++;

                if (consecutiveAbsentDays >= 3)
                {
                    response.Add(new GetAllAbsentStudentResponseDto()
                    {
                        Id = study.Id,
                        StudentId = study.StudentId,
                        Name = study.Student.Contact.Name,
                        Surname = study.Student.Contact.Surname,
                        Father = study.Student.Contact.FatherName,
                        AbsentCount = 3,
                        Class = new GetAllClassDto()
                        {
                            Name = @class.Name,
                            Id = @class.Id
                        }
                    });

                    consecutiveAbsentDays = 0;
                }

                previousAttendanceDate = attendance.ClassSession.Date;
            }
        }

        return response;



    }
}
