using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Space.Application.Abstraction.Common;
using Space.Application.Abstractions;
using Space.Application.Abstractions.Repositories;

namespace Space.Infrastructure.Persistence.Concrete;

internal class UnitOfWork : IUnitOfWork
{

    readonly SpaceDbContext _dbContext;
    readonly IConfiguration _configuration;
    readonly UserManager<User> _userManager;
    readonly RoleManager<Role> _roleManager;
    readonly IMapper _mapper;
    readonly IWebHostEnvironment _webHostEnvironment;

    public UnitOfWork(
        SpaceDbContext dbContext, IConfiguration configuration, UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper, IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }

    #region Repositories
    private IModuleRepository? _moduleRepository;
    private IProgramRepository? _programRepository;
    private IClassRepository? _classRepository;
    private IRoomRepository? _roomRepository;
    private ISessionDetailRepository? _sessionDetailRepository;
    private ISessionRepository? _sessionRepository;
    private IWorkerRepository? _WorkerRepository;
    private IClassModulesWorkerRepository? _classModulesWorkerRepository;
    private IRoleRepository? _roleRepository;
    private IClassSessionRepository? _classSessionRepository;
    private IRoomScheduleRepository? _roomScheduleRepository;
    private IReservationRepository? _reservationRepository;
    private IStudyRepository? _studyRepository;
    private IStudentRepository? _studentRepository;
    private IAttendanceRepository? _attendanceRepository;
    private IHolidayRepository? _holidayRepository;
    private ISupportRepository? _supportRepository;
    private IFileRepository? _fileRepository;

    public IProgramRepository ProgramRepository => _programRepository ??= new ProgramRepository(_dbContext);
    public IModuleRepository ModuleRepository => _moduleRepository ??= new ModuleRepository(_dbContext);
    public IClassRepository ClassRepository => _classRepository ??= new ClassRepository(_dbContext);
    public IRoomRepository RoomRepository => _roomRepository ??= new RoomRepository(_dbContext);
    public ISessionDetailRepository SessionDetailRepository => _sessionDetailRepository ??= new SessionDetailRepository(_dbContext);
    public IWorkerRepository WorkerRepository => _WorkerRepository ??= new WorkerRepository(_dbContext);
    public ISessionRepository SessionRepository => _sessionRepository ??= new SessionRepository(_dbContext);
    public IClassModulesWorkerRepository ClassModulesWorkerRepository => _classModulesWorkerRepository ??= new ClassModuleWorkerRepository(_dbContext);
    public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_dbContext);
    public IClassSessionRepository ClassSessionRepository => _classSessionRepository ??= new ClassSessionRepository(_dbContext);
    public IReservationRepository ReservationRepository => _reservationRepository ??= new ReservationRepository(_dbContext);
    public IRoomScheduleRepository RoomScheduleRepository => _roomScheduleRepository ??= new RoomScheduleRepository(_dbContext);
    public IStudyRepository StudyRepository => _studyRepository ??= new StudyRespository(_dbContext);
    public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(_dbContext);
    public IAttendanceRepository AttendanceRepository => _attendanceRepository ??= new AttendanceRepository(_dbContext);
    public IHolidayRepository HolidayRepository => _holidayRepository ??= new HolidayRepository(_dbContext);
    public ISupportRepository SupportRepository => _supportRepository ??= new SupportRepository(_dbContext);
    public IFileRepository FileRepository => _fileRepository ??= new FileRepository(_dbContext);
    #endregion

    #region Services
    private IEmailService? _emailService;
    private IIdentityService? _identityService;
    private IRoleService? _roleService;
    private ITokenService? _tokenService;
    private IUserService? _userService;
    private IStorageService? _storageService;
    private ITelegramService? _telegramService;
    public IEmailService EmailService => _emailService ??= new EmailService(_configuration, _webHostEnvironment);
    public IIdentityService IdentityService => _identityService ??= new IdentityService(_userManager, _mapper, _configuration);
    public IRoleService RoleService => _roleService ??= new RoleService(_roleManager, _userManager);
    public ITokenService TokenService => _tokenService ??= new TokenService(_configuration);
    public IUserService UserService => _userService ??= new UserService(_userManager);
    public ITelegramService TelegramService => _telegramService ??= new TelegramService(_configuration);

    #endregion

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
