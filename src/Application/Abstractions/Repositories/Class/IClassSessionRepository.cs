namespace Space.Application.Abstractions;

public interface IClassSessionRepository : IRepository<ClassSession>
{
    Task<DateTime> GetLastDateAsync(Guid classId);
    List<ClassSession> GenerateSessions(
                                int programTotalHour,
                                List<CreateClassSessionDto> sessions,
                                DateTime startDate,
                                List<DateTime> holidayDates,
                                Guid classId,
                                Guid roomId);
    Task GenerateAttendanceAsync(
                ICollection<UpdateAttendanceCategorySessionDto> requestSession,
                IEnumerable<ClassSession> classSessions,
                Guid moduleId);
}
