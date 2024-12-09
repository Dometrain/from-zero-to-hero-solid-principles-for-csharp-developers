using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public interface IUserRepository
{
    Task<UserRecord> GetUserAsync(string email, CancellationToken cancellationToken);
    Task<UserRecord> GetUserAsync(int userId, CancellationToken cancellationToken);
    Task<int?> CreateUserAsync(UserRecord user, CancellationToken cancellationToken);
    Task<bool> UpdateUserAsync(int userId, UserRecord user, CancellationToken cancellationToken);
    Task<string> GetHashedPasswordAsync(string email, CancellationToken cancellationToken);
    Task<bool> UpdatePasswordAsync(int userId, string hashedPassword, CancellationToken cancellationToken);
}