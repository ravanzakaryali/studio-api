namespace Space.Application.Abstractions.Services;

public interface IClassSessionService
{
    Task<DateOnly> GetLastDateAsync(Guid classId);

    List<ClassSession> GenerateSessions(
                              int totalHours,
                              List<CreateClassSessionDto> sessions,
                              DateOnly startDate,
                              List<DateOnly> holidayDates,
                              Guid classId,
                              Guid roomId);
    List<ClassSession> GenerateSessions(
                                DateOnly startDate,
                                List<CreateClassSessionDto> sessions,
                                DateOnly endDate,
                                List<DateOnly> holidayDates,
                                Guid classId,
                                Guid roomId);
}
