using Microsoft.AspNetCore.Http;
using Space.Application.Abstractions;
using Space.Application.Helper;
using Space.Domain.Entities;
using System.Security.Claims;
using System.Threading;

namespace Space.Infrastructure.Persistence;

internal class ClassSessionRepository : Repository<ClassSession>, IClassSessionRepository
{
    private readonly IHolidayRepository _holidayRepository;
    private readonly ISpaceDbContext _context;

    public ClassSessionRepository(
        SpaceDbContext context,
        IHolidayRepository holidayRepository) : base(context)
    {
        _holidayRepository = holidayRepository;
        _context = context;
    }

    public async Task GenerateAttendanceAsync(ICollection<UpdateAttendanceCategorySessionDto> requestSession, IEnumerable<ClassSession> classSessions, Guid moduleId)
    {

        List<ClassSession> oldSessions = classSessions.Select(c => (ClassSession)c.Clone()).ToList();


        foreach (ClassSession classSession in classSessions)
        {

            classSession.Status = null;
            classSession.ModuleId = null;
            classSession.Attendances = new List<Attendance>();
            classSession.AttendancesWorkers = new List<AttendanceWorker>();
        }


        List<DateTime> holidayDates = await _holidayRepository.GetDatesAsync();

        Guid classId = classSessions.FirstOrDefault()!.ClassId;

        List<ClassSession> allClassSessions = await _context.ClassSessions
                                                       .Where(c => c.ClassId == classId)
                                                       .OrderByDescending(c => c.Date)
                                                       .ToListAsync();

        DateTime lastDate = allClassSessions.First().Date;


        foreach (UpdateAttendanceCategorySessionDto session in requestSession)
        {
            ClassSession? matchingSession = classSessions.Where(cs => cs.Category == session.Category).FirstOrDefault();
            ClassSession? matchingOldSession = oldSessions.Where(cs => cs.Category == session.Category).FirstOrDefault();

            if (matchingSession == null) break;
            if (matchingOldSession == null) break;

            matchingSession.AttendancesWorkers.AddRange(session.AttendancesWorkers.Select(wa => new AttendanceWorker()
            {
                WorkerId = wa.WorkerId,
                TotalAttendanceHours = wa.IsAttendance ? matchingSession.TotalHour : 0,
                RoleId = wa.RoleId,
            }));

            matchingSession.ModuleId = moduleId;
            matchingSession.Status = session.Status;

            if (session.Status != ClassSessionStatus.Cancelled)
            {
                if (matchingOldSession.Status == ClassSessionStatus.Cancelled)
                {
                    _context.ClassSessions.RemoveRange(allClassSessions.Where(c => c.Date == lastDate).ToList());
                }
                matchingSession.Attendances = session.Attendances.Select(c => new Attendance()
                {
                    StudyId = c.StudentId,
                    Note = c.Note,
                    Status = matchingSession.TotalHour == c.TotalAttendanceHours
                                        ? StudentStatus.Attended
                                        : c.TotalAttendanceHours == 0
                                        ? StudentStatus.Absent
                                        : StudentStatus.Partial,
                    TotalAttendanceHours = c.TotalAttendanceHours
                }).ToList();
            }
            else
            {

                matchingSession.Attendances = new List<Attendance>();
                matchingSession.AttendancesWorkers = new List<AttendanceWorker>();


                if (matchingOldSession.Status != ClassSessionStatus.Cancelled)
                {

                    IEnumerable<DayOfWeek> classDayOfWeek = allClassSessions.DistinctBy(c => c.Date.DayOfWeek).Select(c => c.Date.DayOfWeek);


                    var date2 = lastDate.AddDays(1);
                    DayOfWeek startDateDayOfWeek = date2.DayOfWeek;
                    while (!classDayOfWeek.Any(c => date2.DayOfWeek == c))
                    {
                        date2 = date2.AddDays(1);
                        startDateDayOfWeek = date2.DayOfWeek;
                    }
                    List<ClassSession> generateClassSessions = GenerateSessions(
                        matchingSession.TotalHour, requestSession.Select(r => new CreateClassSessionDto()
                        {
                            Category = r.Category,
                            DayOfWeek = startDateDayOfWeek,
                            End = matchingSession.EndTime,
                            Start = matchingSession.StartTime
                        }).ToList(),
                        date2,
                        holidayDates,
                        matchingSession.ClassId,
                        matchingSession.RoomId.Value);

                    await _context.ClassSessions.AddRangeAsync(generateClassSessions);
                }
            }
        }
    }
    public List<ClassSession> GenerateSessions(
                                int totalHour,
                                List<CreateClassSessionDto> sessions,
                                DateTime startDate,
                                List<DateTime> holidayDates,
                                Guid classId,
                                Guid roomId)
    {

        List<ClassSession> returnClassSessions = new();
        DayOfWeek startDayOfWeek = startDate.DayOfWeek;
        int count = 0;
        while (totalHour > 0)
        {
            foreach (var session in sessions.OrderBy(c => c.DayOfWeek))
            {
                var daysToAdd = ((int)session.DayOfWeek - (int)startDayOfWeek + 7) % 7;
                int numSelectedDays = sessions.Count;

                int hour = (session.End - session.Start).Hours;

                DateTime dateTime = startDate.AddDays(count * 7 + daysToAdd);

                if (holidayDates.Contains(dateTime))
                {
                    continue;
                }

                if (hour != 0)
                {
                    returnClassSessions.Add(new ClassSession()
                    {
                        Category = session.Category,
                        ClassId = classId,
                        StartTime = session.Start,
                        EndTime = session.End,
                        RoomId = roomId,
                        TotalHour = hour,
                        Date = dateTime
                    });
                    if (session.Category != ClassSessionCategory.Lab)
                        totalHour -= hour;
                    if (totalHour <= 0)
                        break;

                }
            }
            count++;

        }
        return returnClassSessions;
    }

    public async Task<DateTime> GetLastDateAsync(Guid classId)
    {
        DateTime lastDate = await _context.ClassSessions
                                                        .Where(c => c.ClassId == classId)
                                                        .Select(c => c.Date)
                                                        .OrderByDescending(c => c)
                                                        .FirstOrDefaultAsync();
        return lastDate;
    }
}
