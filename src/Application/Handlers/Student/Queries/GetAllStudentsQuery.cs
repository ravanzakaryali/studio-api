using System;

namespace Space.Application.Handlers;


public record GetAllStudentsQuery : IRequest<IEnumerable<GetAllStudentsResponseDto>>;


internal class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, IEnumerable<GetAllStudentsResponseDto>>
{
    readonly IUnitOfWork _unitOfWork;
    readonly IStudyRepository _studyRepository;

    public GetAllStudentsQueryHandler(
        IUnitOfWork unitOfWork,
        IStudyRepository studyRepository)
    {
        _unitOfWork = unitOfWork;
        _studyRepository = studyRepository;
    }

    public async Task<IEnumerable<GetAllStudentsResponseDto>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {

        var studies = await _studyRepository.GetAllAsync(q => q.Class.EndDate > DateTime.Now, tracking: false, "Student.Contact", "Class");

        var response = new List<GetAllStudentsResponseDto>();


        foreach (var item in studies)
        {
            if (item.Student != null)
            {
                var hasStudentInData = response.FirstOrDefault(q => q.Id == item.Student.Id);

                if (hasStudentInData != null)
                {
                    var studentClass = new GetAllStudentsClassDto();
                    studentClass.Id = item.Class.Id;
                    studentClass.Name = item.Class?.Name;

                    hasStudentInData.Classes.Add(studentClass);


                }
                else
                {
                    var model = new GetAllStudentsResponseDto();
                    model.Classes = new List<GetAllStudentsClassDto>();
                    model.Id = item.Student.Id;
                    model.EMail = item.Student.Email;
                    model.Name = item.Student.Contact?.Name;
                    model.Surname = item.Student.Contact?.Surname;
                    model.Phone = item.Student.Contact?.Phone;

                    var studentClass = new GetAllStudentsClassDto();
                    studentClass.Id = item.Class.Id;
                    studentClass.Name = item.Class?.Name;

                    model.Classes.Add(studentClass);


                    response.Add(model);
                }
            }
        }
        return response;
    }
}


