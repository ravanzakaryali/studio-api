using Space.Domain.Entities;
using System.Globalization;

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

            int consecutiveAbsentWeeks = 0;
            DateTime previousAttendanceDate = DateTime.MinValue;
            GetAllAbsentStudentResponseDto currentStudentResponse = null;
            bool isConsecutive = false; // Arka arkaya gelmeme durumu kontrolü

            foreach (var attendance in orderedAttendances)
            {
                // Eğer iki ders arasındaki hafta farkı 1'den fazlaysa, arka arkaya gelmeme durumu bozuldu demektir.
                if (CalculateWeekDifference(previousAttendanceDate, attendance.ClassSession.Date) > 1)
                {
                    // Arka arkaya gelmeme durumu bozulduğunda, currentStudentResponse'ı response listesine ekle
                    if (isConsecutive && consecutiveAbsentWeeks >= 3)
                    {
                        response.Add(currentStudentResponse);
                    }

                    // Arka arkaya gelmeme durumu bozulduğunda, yeni bir kayıt başlat
                    currentStudentResponse = new GetAllAbsentStudentResponseDto()
                    {
                        Id = study.Id,
                        StudentId = study.StudentId,
                        Name = study.Student.Contact.Name,
                        Surname = study.Student.Contact.Surname,
                        Father = study.Student.Contact.FatherName,
                        Class = new GetAllClassDto()
                        {
                            Name = @class.Name,
                            Id = @class.Id
                        }
                    };

                    // Yeni dönemin başladığını işaretle
                    isConsecutive = true;

                    // Yeni dönemdeki ilk dersi saymaya başla
                    consecutiveAbsentWeeks = 1;
                }
                else
                {
                    consecutiveAbsentWeeks++;
                }

                // Her defasında öğrencinin son katıldığı günü güncelle
                previousAttendanceDate = attendance.ClassSession.Date;
                currentStudentResponse.AbsentCount = consecutiveAbsentWeeks;
            }

            // Döngü sonunda, son öğrenci kaydını response listesine ekleyin
            if (isConsecutive && consecutiveAbsentWeeks >= 3)
            {
                response.Add(currentStudentResponse);
            }
        }

        return response;

        // İki tarih arasındaki hafta farkını hesaplayan fonksiyon
        int CalculateWeekDifference(DateTime startDate, DateTime endDate)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            int startWeek = cal.GetWeekOfYear(startDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
            int endWeek = cal.GetWeekOfYear(endDate, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

            return endWeek - startWeek;
        }


    }
}
