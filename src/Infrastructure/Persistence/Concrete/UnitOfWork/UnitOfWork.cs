using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Space.Application.Abstraction.Common;
using Space.Application.Abstractions;
using Space.Application.Abstractions.Repositories;

namespace Space.Infrastructure.Persistence.Concrete;

internal class UnitOfWork : IUnitOfWork
{

    readonly ISpaceDbContext _dbContext;
    readonly IConfiguration _configuration;
    readonly UserManager<User> _userManager;
    readonly RoleManager<Role> _roleManager;
    readonly IMapper _mapper;
    readonly IWebHostEnvironment _webHostEnvironment;
   

    public UnitOfWork(
        ISpaceDbContext dbContext,
        IConfiguration configuration,
        UserManager<User> userManager,
        RoleManager<Role> roleManager,
        IMapper mapper, 
        IWebHostEnvironment webHostEnvironment)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _userManager = userManager;
        _roleManager = roleManager;
        _mapper = mapper;
        _webHostEnvironment = webHostEnvironment;
    }
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
