using Microsoft.AspNetCore.Identity;
using Store.Application.Models;
using Store.Common.Results;
using Store.Common.Helpers;

namespace Store.Application.Services;

public class PasswordService
{
    private readonly PasswordHasher<User> _passwordHasher;
    public PasswordService()
    {
        _passwordHasher = new PasswordHasher<User>();
    }
    public Result VerifyHashedPassword(string hashPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(new User(), hashPassword, password);
        if (result == PasswordVerificationResult.Success)
            return new SuccessResult();

        return new InvalidResult("Password did not match");
    }

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(new User(), password);
    }
}