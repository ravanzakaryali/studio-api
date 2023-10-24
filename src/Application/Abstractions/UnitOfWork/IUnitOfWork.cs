using Space.Application.Abstraction.Common;
using Space.Application.Abstractions.Repositories;

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

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
