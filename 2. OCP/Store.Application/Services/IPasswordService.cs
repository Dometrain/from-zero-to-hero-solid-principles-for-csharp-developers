using Store.Common.Results;

namespace Store.Application.Services;

public interface IPasswordService
{
    Task<Result> VerifyPassword(string email, string password, CancellationToken cancellationToken);
    string HashPassword(string password);
    Task<Result> UpdatePasswordAsync(int userId, string password, CancellationToken cancellationToken);
}