using Store.Api.Contracts.Requests;
using Store.Api.Contracts.Responses;
using Store.Application.Models;

namespace Store.Api.Mappers;

public static class UserMapper
{
    public static AdminUserResponse MapAdmin(this User user) => new AdminUserResponse
    {
        UserId = user.UserId,
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        CreatedAt = user.CreatedAt,
        DeliveryAddress = user.DeliveryAddress,
        CountryCode = user.CountryCode,
        Roles = user.Roles
    };

    public static UserResponse Map(this User user) => new UserResponse
    {
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        DeliveryAddress = user.DeliveryAddress,
        CountryCode = user.CountryCode
    };

    public static User Map(this CreateUserRequest user) => new User
    {
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        DeliveryAddress = user.DeliveryAddress,
        CountryCode = user.CountryCode
    };

    public static User Map(this UpdateUserRequest user) => new User
    {
        Email = user.Email,
        FirstName = user.FirstName,
        LastName = user.LastName,
        DeliveryAddress = user.DeliveryAddress,
        CountryCode = user.CountryCode
    };
}