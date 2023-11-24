namespace Space.Application.Abstractions.Services;

public interface IClassSessionService
{
    Task<DateTime> GetLastDateAsync(Guid classId);

    Task GenerateAttendanceAsync(
                ICollection<UpdateAttendanceCategorySessionDto> requestSession,
                IEnumerable<ClassTimeSheet> classSessions,
                Guid moduleId);
    List<ClassTimeSheet> GenerateSessions(
                              int programTotalHour,
                              List<CreateClassSessionDto> sessions,
                              DateTime startDate,
                              List<DateTime> holidayDates,
                              Guid classId,
                              Guid roomId);
    List<ClassTimeSheet> GenerateSessions(
                                DateTime startDate,
                                List<CreateClassSessionDto> sessions,
                                DateTime endDate,
                                List<DateTime> holidayDates,
                                Guid classId,
                                Guid roomId);
}
