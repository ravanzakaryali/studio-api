using Space.Application.Abstraction.Common;

namespace Space.Application.Abstractions;

public interface IUnitOfWork
{
    #region Services
    public IEmailService EmailService { get; }
    public IIdentityService IdentityService { get; }
    public IRoleService RoleService { get; }
    public ITokenService TokenService { get; }
    public IUserService UserService { get; }
    public ITelegramService TelegramService { get; }
    #endregion

    #region Entity Services
    public IClassSessionService ClassSessionService { get; }
    public IModuleService ModuleService { get; }
    public IHolidayService HolidayService { get; }
    public IClassService ClassService { get; }
    #endregion
}
