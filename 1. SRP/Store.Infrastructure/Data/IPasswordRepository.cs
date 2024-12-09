namespace Store.Infrastructure.Data;

public interface IPasswordRepository
{
    Task<string> GetHashedPasswordAsync(string email, CancellationToken cancellationToken);
    Task<bool> UpdatePasswordAsync(int userId, string hashedPassword, CancellationToken cancellationToken);
}