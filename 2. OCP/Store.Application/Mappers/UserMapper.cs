using Store.Application.Models;
using Store.Infrastructure.Models;

namespace Store.Application.Mappers;

public static class UserMapper
{
    public static User Map(this UserRecord user)
    {
        return new User
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DeliveryAddress = user.DeliveryAddress,
            CountryCode = user.CountryCode,
            Roles = user.IsAdmin ? new[] { Roles.AdminRole, Roles.DefaultRole } : new[] { Roles.DefaultRole },
            CreatedAt = user.CreatedAt
        };
    }

    public static UserRecord Map(this User user, string hashedPassword)
    {
        return new UserRecord
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            HashedPassword = hashedPassword,
            DeliveryAddress = user.DeliveryAddress,
            CountryCode = user.CountryCode,
            CreatedAt = user.CreatedAt
        };
    }

    public static UserRecord Map(this User user)
    {
        return new UserRecord
        {
            UserId = user.UserId,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            DeliveryAddress = user.DeliveryAddress,
            CountryCode = user.CountryCode,
            CreatedAt = user.CreatedAt
        };
    }
}