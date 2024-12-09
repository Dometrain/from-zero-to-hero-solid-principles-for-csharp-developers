using Dapper;
using Npgsql;
using Store.Common.Helpers;
using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public class OrderRepository : IReadOrders, IWriteOrder
{
    private readonly NpgsqlDataSource _database;

    public OrderRepository(NpgsqlDataSource database)
    {
        _database = database.NotNull();
    }

    public async Task<int?> CreateOrderAsync(OrderRecord order, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string sql = @$"
            INSERT INTO public.orders(user_id, order_total, delivery_cost, tax)
	        VALUES (@UserId, @OrderTotal, @DeliveryCos, @Tax)
            RETURNING order_id;";

            var orderId = await connection.QueryFirstAsync<int>(sql, new { order.UserId, order.OrderTotal, order.DeliveryCost, order.Tax }, transaction);
            var result = await AddItemsToOrderAsync(connection, transaction, orderId, order.Items, cancellationToken);

            if (result)
            {
                await transaction.CommitAsync();
                return orderId;
            }

            await transaction.RollbackAsync();
            return null;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private async Task<bool> AddItemsToOrderAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, int orderId, IEnumerable<ItemRecord> items, CancellationToken cancellationToken)
    {
        const string sql = @$"
            INSERT INTO public.ordered_products(order_id, product_id, quantity)
            VALUES (@OrderId, @ProductId, @Quantity);";

        var itemsToOrder = items.Select(x => new { OrderId = orderId, x.Product.ProductId, x.Quantity });

        var result = await connection.ExecuteAsync(sql, itemsToOrder, transaction);
        return result >= 1;
    }

    public async Task<OrderRecord> GetOrderAsync(int userId, int orderId, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                o.order_id AS {nameof(OrderRecord.OrderId)},
                o.user_id AS {nameof(OrderRecord.UserId)},
                o.delivery_cost AS {nameof(OrderRecord.DeliveryCost)},
                o.tax AS {nameof(OrderRecord.Tax)},
                o.created_at AS {nameof(OrderRecord.CreatedAt)},
                (SELECT json_agg(i) FROM (
                    SELECT
                        op.ordered_product_id AS {nameof(ItemRecord.ItemId)},
                        op.quantity AS {nameof(ItemRecord.Quantity)},
                        op.created_at AS {nameof(ItemRecord.CreatedAt)},
                        (SELECT to_json(p) FROM (
                            SELECT
                                product_id AS {nameof(ProductRecord.ProductId)},
                                name AS {nameof(ProductRecord.Name)},
                                price AS {nameof(ProductRecord.Price)},
                                description AS {nameof(ProductRecord.Description)},
                                created_at AS {nameof(ProductRecord.CreatedAt)}
                            FROM public.products
                            WHERE product_id = op.product_id) AS p) AS {nameof(ItemRecord.Product)}
                    FROM public.ordered_products op
                    WHERE op.order_id = o.order_id
                ) AS i) AS {nameof(OrderRecord.Items)}
            FROM public.orders o
            WHERE o.order_id = @orderId AND o.user_id = @userId;
        ";

        return await connection.QueryFirstOrDefaultAsync<OrderRecord>(sql, new { userId, orderId });
    }

    public async Task<PagedRecord<OrderRecord>> GetOrdersAsync(int userId, int page, int pageSize, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                (SELECT (COUNT(*)/@pageSize) + 1 FROM orders WHERE user_id = @userId) AS {nameof(PagedRecord<OrderRecord>.TotalPages)},
                @page AS {nameof(PagedRecord<OrderRecord>.Page)},
                @pageSize AS {nameof(PagedRecord<OrderRecord>.PageSize)},
                (SELECT json_agg(j) FROM (SELECT
                    o.order_id AS {nameof(OrderRecord.OrderId)},
                    o.user_id AS {nameof(OrderRecord.UserId)},
                    o.delivery_cost AS {nameof(OrderRecord.DeliveryCost)},
                    o.tax AS {nameof(OrderRecord.Tax)},
                    o.created_at AS {nameof(OrderRecord.CreatedAt)},
                    (SELECT json_agg(i) FROM (
                        SELECT
                            op.ordered_product_id AS {nameof(ItemRecord.ItemId)},
                            op.quantity AS {nameof(ItemRecord.Quantity)},
                            op.created_at AS {nameof(ItemRecord.CreatedAt)},
                            (SELECT to_json(p) FROM (
                                SELECT
                                    product_id AS {nameof(ProductRecord.ProductId)},
                                    name AS {nameof(ProductRecord.Name)},
                                    price AS {nameof(ProductRecord.Price)},
                                    description AS {nameof(ProductRecord.Description)},
                                    created_at AS {nameof(ProductRecord.CreatedAt)}
                                FROM public.products
                                WHERE product_id = op.product_id) AS p) AS {nameof(ItemRecord.Product)}
                        FROM public.ordered_products op
                        WHERE op.order_id = o.order_id
                    ) AS i) AS {nameof(OrderRecord.Items)}
                FROM public.orders o
                WHERE o.user_id = @userId
                ORDER BY order_id DESC
                OFFSET @offset
                LIMIT @pageSize) AS j) AS {nameof(PagedRecord<OrderRecord>.Data)};
        ";

        return await connection.QueryFirstOrDefaultAsync<PagedRecord<OrderRecord>>(sql, new { userId, pageSize, page, offset = (page - 1) * pageSize });
    }

    public async Task<IEnumerable<OrderReportRecord>> GetOrderReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);
        const string sql = @$"
            SELECT
                date(created_at) AS {nameof(OrderReportRecord.OrderDate)},
                count(*) AS {nameof(OrderReportRecord.OrderCount)},
                sum(order_total) AS {nameof(OrderReportRecord.OrderTotal)}
            FROM public.orders
            WHERE created_at >= @from AND created_at < @to
            GROUP BY date(created_at);
        ";
        return await connection.QueryAsync<OrderReportRecord>(sql, new { from, to });
    }
}