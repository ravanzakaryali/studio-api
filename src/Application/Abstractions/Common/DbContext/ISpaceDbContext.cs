namespace Space.Application.Abstractions;

public interface ISpaceDbContext
{

    #region Class
    DbSet<Class> Classes { get; }
    DbSet<ClassTimeSheet> ClassTimeSheets { get; }
    DbSet<ClassSession> ClassSessions { get; }
    DbSet<Attendance> Attendances { get; }
    DbSet<Room> Rooms { get; }
    DbSet<Session> Sessions { get; }
    DbSet<SessionDetail> SessionDetails { get; }
    DbSet<ClassModulesWorker> ClassModulesWorkers { get; }
    DbSet<ClassExtraModulesWorkers> ClassExtraModulesWorkers { get; }
    DbSet<AttendanceWorker> AttendancesWorkers { get; }
    DbSet<HeldModule> HeldModules { get; }
    #endregion
    #region Common
    DbSet<University> Universities { get; }
    DbSet<E.File> Files { get; }

    #endregion
    #region Identity

    #endregion
    #region Program
    DbSet<Module> Modules { get; }
    DbSet<Program> Programs { get; }
    DbSet<ExtraModule> ExtraModules { get; }
    #endregion
    #region User
    DbSet<Worker> Workers { get; }
    DbSet<Role> Roles { get; }
    #endregion
    #region Student
    DbSet<Contact> Contacts { get; }
    DbSet<Student> Students { get; }
    DbSet<Study> Studies { get; }
    #endregion
    DbSet<Reservation> Reservations { get; }
    DbSet<RoomSchedule> RoomSchedules { get; }

    DbSet<Holiday> Holidays { get; }
    DbSet<Support> Supports { get; }


    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
