using Npgsql;
using Store.Infrastructure.Infrastructure;

namespace Store.Infrastructure.Data;

// Not recommend way to set up a Database. Shown purely for education purposes.
public class DataSource
{
    public DataSource()
    {
        var connectionString = ConfigHelper.GetCurrentSettings("ConnectionStrings:Postgres").Value;
        Database = NpgsqlDataSource.Create(connectionString);
    }

    public NpgsqlDataSource Database { get; }
}