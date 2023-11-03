namespace Space.Application.Abstractions;

public interface IClassSessionRepository : IRepository<ClassSession>
{
    Task<DateTime> GetLastDateAsync(Guid classId);
  
    Task GenerateAttendanceAsync(
                ICollection<UpdateAttendanceCategorySessionDto> requestSession,
                IEnumerable<ClassSession> classSessions,
                Guid moduleId);
    List<ClassSession> GenerateSessions(
                              int programTotalHour,
                              List<CreateClassSessionDto> sessions,
                              DateTime startDate,
                              List<DateTime> holidayDates,
                              Guid classId,
                              Guid roomId);
    List<ClassSession> GenerateSessions(
                                DateTime startDate,
                                List<CreateClassSessionDto> sessions,
                                DateTime endDate,
                                List<DateTime> holidayDates,
                                Guid classId,
                                Guid roomId);
}
