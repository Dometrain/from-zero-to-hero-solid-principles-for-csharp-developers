using Dapper;
using Npgsql;
using Store.Common.Helpers;
using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public class PasswordRepository : IPasswordRepository
{
    private readonly NpgsqlDataSource _database;

    public PasswordRepository(NpgsqlDataSource database)
    {
        _database = database.NotNull();
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
