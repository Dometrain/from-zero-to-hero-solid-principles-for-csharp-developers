using Store.Common.Results;
using Store.Common.Helpers;
using Store.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Store.Application.Models;

namespace Store.Application.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordService _passwordService;
    public AuthService()
    {
        _userRepository = new UserRepository();
        _passwordService = new PasswordService();
    }

    public async Task<Result> VerifyPassword(string email, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = await _userRepository.GetHashedPasswordAsync(email, cancellationToken);

        if (hashedPassword != null)
        {
            var result = _passwordService.VerifyHashedPassword(hashedPassword, password);
            if (result.Success)
                return new SuccessResult();
        }

        return new InvalidResult("Invalid email or password");
    }
}
