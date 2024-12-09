using Dapper;
using Npgsql;
using Store.Common.Helpers;
using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public class UserRepository : IUserRepository
{
    private readonly NpgsqlDataSource _database;

    public UserRepository(NpgsqlDataSource database)
    {
        _database = database.NotNull();
    }

    public async Task<UserRecord> GetUserAsync(string email, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                user_id AS {nameof(UserRecord.UserId)},
                first_name  AS {nameof(UserRecord.FirstName)},
                last_name AS {nameof(UserRecord.LastName)},
                email AS {nameof(UserRecord.Email)},
                delivery_address AS {nameof(UserRecord.DeliveryAddress)},
                country_code AS {nameof(UserRecord.CountryCode)},
                is_admin AS {nameof(UserRecord.IsAdmin)},
                created_at AS {nameof(UserRecord.CreatedAt)}
            FROM users
            WHERE email = @email;";

        return await connection.QueryFirstOrDefaultAsync<UserRecord>(sql, new { email });
    }

    public async Task<UserRecord> GetUserAsync(int userId, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                user_id AS {nameof(UserRecord.UserId)},
                first_name  AS {nameof(UserRecord.FirstName)},
                last_name AS {nameof(UserRecord.LastName)},
                email AS {nameof(UserRecord.Email)},
                delivery_address AS {nameof(UserRecord.DeliveryAddress)},
                country_code AS {nameof(UserRecord.CountryCode)},
                is_admin AS {nameof(UserRecord.IsAdmin)},
                created_at AS {nameof(UserRecord.CreatedAt)}
            FROM users
            WHERE user_id = @userId;";

        return await connection.QueryFirstOrDefaultAsync<UserRecord>(sql, new { userId });
    }

    public async Task<int?> CreateUserAsync(UserRecord user, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @"
            INSERT INTO public.users(first_name, last_name, email, password, delivery_address, country_code, is_admin)
	        VALUES (@FirstName, @LastName, @Email, @HashedPassword, @DeliveryAddress, @CountryCode, @IsAdmin)
            ON CONFLICT (email)
            DO NOTHING
            RETURNING user_id;";

        var result = await connection.QueryFirstOrDefaultAsync<int?>(sql, user);
        return result;
    }

    public async Task<bool> UpdateUserAsync(int userId, UserRecord user, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @"
            UPDATE public.users
            SET first_name = @FirstName,
                last_name = @LastName,
                email = @Email,
                delivery_address = @DeliveryAddress,
                country_code = @CountryCode
            WHERE user_id = @UserId
            AND NOT EXISTS (SELECT 1 FROM public.users WHERE email = @Email);";

        var param = new { UserId = userId, user.FirstName, user.LastName, user.Email, user.DeliveryAddress, user.CountryCode };

        var result = await connection.ExecuteAsync(sql, param);
        return result >= 1;
    }

    public async Task<string> GetHashedPasswordAsync(string email, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT password
            FROM users
            WHERE email = @email;";

        return await connection.QueryFirstOrDefaultAsync<string>(sql, new { email });
    }

    public async Task<bool> UpdatePasswordAsync(int userId, string hashedPassword, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @"
            UPDATE public.users
            SET password = @hashedPassword
            WHERE user_id = @userId;";

        var result = await connection.ExecuteAsync(sql, new { userId, hashedPassword });
        return result >= 1;
    }
}
