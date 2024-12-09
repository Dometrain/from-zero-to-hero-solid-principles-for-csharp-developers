using Microsoft.AspNetCore.Identity;
using Store.Application.Models;
using Store.Common.Results;
using Store.Common.Helpers;
using Store.Infrastructure.Data;

namespace Store.Application.Services;

public class PasswordService : IPasswordService
{
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IPasswordRepository _passwordRespository;
    public PasswordService(IPasswordHasher<User> passwordHasher, IPasswordRepository passwordRespository)
    {
        _passwordHasher = passwordHasher.NotNull();
        _passwordRespository = passwordRespository.NotNull();
    }

    public async Task<Result> UpdatePasswordAsync(int userId, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = HashPassword(password);
        var result = await _passwordRespository.UpdatePasswordAsync(userId, hashedPassword, cancellationToken);
        if (!result)
            return new ErrorResult("An error occurred updating password.");

        return new SuccessResult();
    }

    public async Task<Result> VerifyPassword(string email, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = await _passwordRespository.GetHashedPasswordAsync(email, cancellationToken);

        if (hashedPassword != null)
        {
            var result = VerifyHashedPassword(hashedPassword, password);
            if (result)
                return new SuccessResult();
        }

        return new InvalidResult("Invalid email or password");
    }

    private bool VerifyHashedPassword(string hashPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(new User(), hashPassword, password);
        return result == PasswordVerificationResult.Success;
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(new User(), password);
    }
}