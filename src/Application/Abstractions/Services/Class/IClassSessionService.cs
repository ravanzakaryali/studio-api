namespace Space.Application.Abstractions.Services;

public interface IClassSessionService
{
    Task<DateOnly> GetLastDateAsync(int classId);

    List<ClassSession> GenerateSessions(
                              int totalHours,
                              List<CreateClassSessionDto> sessions,
                              DateOnly startDate,
                              List<DateOnly> holidayDates,
                              int classId,
                              int roomId);
    List<ClassSession> GenerateSessions(
                                DateOnly startDate,
                                List<CreateClassSessionDto> sessions,
                                DateOnly endDate,
                                List<DateOnly> holidayDates,
                                int classId,
                                int roomId);
}
