using Store.Application.Mappers;
using Store.Application.Models;
using Store.Common.Results;
using Store.Common.Helpers;
using Store.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;

namespace Store.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository.NotNull();
        _passwordHasher = passwordHasher.NotNull();
    }

    public async Task<Result<User>> GetUserAsync(string email, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(email, cancellationToken);
        if (user == null)
            return new NotFoundResult<User>();

        return new SuccessResult<User>(user.Map());
    }

    public async Task<Result<User>> GetUserAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserAsync(userId, cancellationToken);
        if (user == null)
            return new NotFoundResult<User>();

        return new SuccessResult<User>(user.Map());
    }

    public async Task<Result<User>> CreateUserAsync(User user, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = HashPassword(password);
        var userId = await _userRepository.CreateUserAsync(user.Map(hashedPassword), cancellationToken);
        if (userId == null)
            return new InvalidResult<User>("Invalid user details. Please update and try again.");

        var userResult = await _userRepository.GetUserAsync(userId.Value, cancellationToken);
        if (userResult == null)
            return new NotFoundResult<User>();

        return new SuccessResult<User>(userResult.Map());
    }

    public async Task<Result<User>> UpdateUserAsync(int userId, User user, CancellationToken cancellationToken)
    {
        var result = await _userRepository.UpdateUserAsync(userId, user.Map(), cancellationToken);
        if (!result)
            return new InvalidResult<User>("Invalid user details. Please update and try again.");

        var userResult = await _userRepository.GetUserAsync(userId, cancellationToken);
        if (userResult == null)
            return new NotFoundResult<User>();

        return new SuccessResult<User>(userResult.Map());
    }

    public async Task<Result> UpdatePasswordAsync(int userId, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = HashPassword(password);
        var result = await _userRepository.UpdatePasswordAsync(userId, hashedPassword, cancellationToken);
        if (!result)
            return new ErrorResult("An error occurred updating password.");

        return new SuccessResult();
    }

    public async Task<Result> VerifyPassword(string email, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = await _userRepository.GetHashedPasswordAsync(email, cancellationToken);

        if (hashedPassword != null)
        {
            var result = VerifyHashedPassword(hashedPassword, password);
            if (result)
                return new SuccessResult();
        }

        return new InvalidResult("Invalid email or password");
    }

    public async Task<Result<User>> CreateAdminUserAsync(User user, string password, CancellationToken cancellationToken)
    {
        var hashedPassword = HashPassword(password);
        var newUser = user.Map(hashedPassword);
        newUser.IsAdmin = true;
        var userId = await _userRepository.CreateUserAsync(newUser, cancellationToken);
        if (userId == null)
            return new InvalidResult<User>("Invalid user details. Please update and try again.");

        var userResult = await _userRepository.GetUserAsync(userId.Value, cancellationToken);
        if (userResult == null)
            return new NotFoundResult<User>();

        return new SuccessResult<User>(userResult.Map());
    }

    private bool VerifyHashedPassword(string hashPassword, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(new User(), hashPassword, password);
        return result == PasswordVerificationResult.Success;
    }

    private string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(new User(), password);
    }
}