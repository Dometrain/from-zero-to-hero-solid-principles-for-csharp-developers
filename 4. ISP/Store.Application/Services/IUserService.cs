using Store.Application.Models;
using Store.Common.Results;

namespace Store.Application.Services;

public interface IUserService
{
    Task<Result<User>> GetUserAsync(string email, CancellationToken cancellationToken);
    Task<Result<User>> GetUserAsync(int userId, CancellationToken cancellationToken);
    Task<Result<User>> CreateUserAsync(User user, string password, CancellationToken cancellationToken);
    Task<Result<User>> UpdateUserAsync(int userId, User user, CancellationToken cancellationToken);
    Task<Result> VerifyPassword(string email, string password, CancellationToken cancellationToken);
    Task<Result> UpdatePasswordAsync(int userId, string password, CancellationToken cancellationToken);
    Task<Result<User>> CreateAdminUserAsync(User user, string password, CancellationToken cancellationToken);
}