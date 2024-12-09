using Dapper;
using Npgsql;
using Store.Common.Helpers;
using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public class ProductRepository : IReadProducts
{
    private readonly NpgsqlDataSource _database;

    public ProductRepository(NpgsqlDataSource database)
    {
        _database = database.NotNull();
    }

    public async Task<ProductRecord> GetProductAsync(int productId, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                product_id AS {nameof(ProductRecord.ProductId)},
                name AS {nameof(ProductRecord.Name)},
                price AS {nameof(ProductRecord.Price)},
                description AS {nameof(ProductRecord.Description)},
                created_at AS {nameof(ProductRecord.CreatedAt)}
            FROM products
            WHERE product_id = @productId;
        ";

        return await connection.QueryFirstOrDefaultAsync<ProductRecord>(sql, new { productId });
    }

    public async Task<PagedRecord<ProductRecord>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                (SELECT (COUNT(*)/@pageSize) + 1 FROM products) AS {nameof(PagedRecord<ProductRecord>.TotalPages)},
                @page AS {nameof(PagedRecord<ProductRecord>.Page)},
                @pageSize AS {nameof(PagedRecord<ProductRecord>.PageSize)},
                (SELECT json_agg(p.*) FROM (
                    SELECT
                        product_id AS {nameof(ProductRecord.ProductId)},
                        name AS {nameof(ProductRecord.Name)},
                        price AS {nameof(ProductRecord.Price)},
                        description AS {nameof(ProductRecord.Description)},
                        created_at AS {nameof(ProductRecord.CreatedAt)}
                    FROM products
                    ORDER BY product_id
                    OFFSET @offset
                    LIMIT @pageSize
                ) AS p) AS {nameof(PagedRecord<ProductRecord>.Data)}
        ";

        return await connection.QueryFirstOrDefaultAsync<PagedRecord<ProductRecord>>(sql, new { pageSize, page, offset = (page - 1) * pageSize });
    }
}
