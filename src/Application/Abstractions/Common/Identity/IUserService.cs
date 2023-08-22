namespace Space.Application.Abstractions;

public interface IUserService
{
    Task<User> FindById(Guid id); 
    Task<User> FindByEmailAsync(string email);
    Task<User> FindByNameAsync(string username);
    Task PasswordAssignAsync(Guid id, string password);
    Task UpdateRefreshToken(string refreshToken, Token token, int addMinute = 15);
    Task CreateWorkerAsync(Worker worker);
}
