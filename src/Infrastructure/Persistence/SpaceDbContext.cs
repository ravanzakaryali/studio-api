﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Space.Domain.Entities;

namespace Space.Infrastructure.Persistence;

public class SpaceDbContext :
    IdentityDbContext<User, Role, int, IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>,
    ISpaceDbContext
{
    private readonly AuditableEntitySaveChangesInterceptor _auditableEntitySaveChangesInterceptor;
    public SpaceDbContext(
        DbContextOptions<SpaceDbContext> options,
        AuditableEntitySaveChangesInterceptor auditableEntitySaveChangesInterceptor
        ) : base(options)
    {
        _auditableEntitySaveChangesInterceptor = auditableEntitySaveChangesInterceptor;
    }
    #region Class
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<ClassSession> ClassSessions => Set<ClassSession>();
    public DbSet<Room> Rooms => Set<Room>();

    public DbSet<HeldModule> HeldModules => Set<HeldModule>();
    public DbSet<Session> Sessions => Set<Session>();
    public DbSet<SessionDetail> SessionDetails => Set<SessionDetail>();
    public DbSet<ClassModulesWorker> ClassModulesWorkers => Set<ClassModulesWorker>();
    public DbSet<ClassExtraModulesWorkers> ClassExtraModulesWorkers => Set<ClassExtraModulesWorkers>();
    public DbSet<ClassTimeSheet> ClassTimeSheets => Set<ClassTimeSheet>();
    public DbSet<Attendance> Attendances => Set<Attendance>();
    public DbSet<AttendanceWorker> AttendancesWorkers => Set<AttendanceWorker>();
    #endregion
    #region Common
    public DbSet<University> Universities => Set<University>();
    public DbSet<Holiday> Holidays => Set<Holiday>();
    public DbSet<Support> Supports => Set<Support>();
    public DbSet<E.File> Files => Set<E.File>();
    public DbSet<Notification> Notifications => Set<Notification>();
    #endregion
    #region Identity

    #endregion
    #region Program
    public DbSet<Module> Modules => Set<Module>();
    public DbSet<ExtraModule> ExtraModules => Set<ExtraModule>();
    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Project> Projects => Set<Project>();
    #endregion
    #region Student
    public DbSet<Contact> Contacts => Set<Contact>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Study> Studies => Set<Study>();
    #endregion
    #region User
    public DbSet<Worker> Workers => Set<Worker>();

    #endregion
    #region Schedule
    public DbSet<RoomSchedule> RoomSchedules => Set<RoomSchedule>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<ApplicationModule> ApplicationModules => Set<ApplicationModule>();
    public DbSet<EndpointAccess> EndpointAccesses => Set<EndpointAccess>();
    public DbSet<Endpoint> Endpoints => Set<Endpoint>();
    public DbSet<PermissionAccess> PermissionAccesses => Set<PermissionAccess>();
    public DbSet<PermissionLevel> PermissionLevels => Set<PermissionLevel>();
    public DbSet<PermissionGroup> PermissionGroups => Set<PermissionGroup>();
    public DbSet<PermissionGroupPermissionLevelAppModule> PermissionGroupPermissionLevelAppModules => Set<PermissionGroupPermissionLevelAppModule>();
    public DbSet<WorkerPermissionLevelAppModule> WorkerPermissionLevelAppModules => Set<WorkerPermissionLevelAppModule>();
    public DbSet<SupportCategory> SupportCategories => Set<SupportCategory>();

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_auditableEntitySaveChangesInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Reflection.Assembly.GetExecutingAssembly());

    }
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

}



public class NullableTimeOnlyConverter : ValueConverter<TimeOnly?, TimeSpan?>
{
    public NullableTimeOnlyConverter() : base(
        t => t == null
            ? null
            : new TimeSpan?(t.Value.ToTimeSpan()),
        t => t == null
            ? null
            : new TimeOnly?(TimeOnly.FromTimeSpan(t.Value)))
    { }
}
