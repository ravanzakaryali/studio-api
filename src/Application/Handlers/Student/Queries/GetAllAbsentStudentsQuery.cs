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
        int absentCount = 0;

        foreach (Study study in @class.Studies.Where(c=>c.StudyType != StudyType.Completion))
        {
            IGrouping<int, Attendance>? groupBy = study.Attendances
                .Where(c => c.TotalAttendanceHours == 0 && c.ClassSession.Category != ClassSessionCategory.Lab )
                .GroupBy(c => c.TotalAttendanceHours).FirstOrDefault();
            if( groupBy != null && groupBy.Count() >= 3)
            {
                response.Add(new GetAllAbsentStudentResponseDto()
                {
                    Id = study.Id,
                    StudentId = study.StudentId,
                    Name = study.Student.Contact.Name,
                    Surname = study.Student.Contact.Surname,
                    AbsentCount = groupBy.Count(),
                    Class = new GetAllClassDto()
                    {
                        Name = @class.Name,
                        Id = @class.Id
                    }
                });
            }
        }

        return response;
    }
}
