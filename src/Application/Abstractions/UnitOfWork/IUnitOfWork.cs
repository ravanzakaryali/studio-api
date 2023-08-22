using Space.Application.Abstraction.Common;
using Space.Application.Abstractions.Repositories;

namespace Space.Application.Abstractions;

public interface IUnitOfWork
{

    #region Repositories
    IProgramRepository ProgramRepository { get; }
    IModuleRepository ModuleRepository { get; }
    IClassRepository ClassRepository { get; }
    IRoomRepository RoomRepository { get; }
    ISessionRepository SessionRepository { get; }
    ISessionDetailRepository SessionDetailRepository { get; }
    IWorkerRepository WorkerRepository { get; }
    IClassModulesWorkerRepository ClassModulesWorkerRepository { get; }
    IRoleRepository RoleRepository { get; }
    IClassSessionRepository ClassSessionRepository { get; }
    IStudyRepository StudyRepository { get; }
    IReservationRepository ReservationRepository { get; }
    IStudentRepository StudentRepository { get; }
    IAttendanceRepository AttendanceRepository { get; }
    IRoomScheduleRepository RoomScheduleRepository { get; }
    IHolidayRepository HolidayRepository { get; }
    ISupportRepository SupportRepository { get; }
    IFileRepository FileRepository { get; }
    #endregion

    #region Services
    public IEmailService EmailService { get; }
    public IIdentityService IdentityService { get; }
    public IRoleService RoleService { get; }
    public ITokenService TokenService { get; }
    public IUserService UserService { get; }
    public ITelegramService TelegramService { get; }
    #endregion

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
