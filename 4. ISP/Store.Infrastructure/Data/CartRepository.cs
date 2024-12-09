using Dapper;
using Npgsql;
using Store.Common.Helpers;
using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public class CartRepository : IReadCart, IWriteCart
{
    private readonly NpgsqlDataSource _database;

    public CartRepository(NpgsqlDataSource database)
    {
        _database = database.NotNull();
    }

    public async Task<bool> AddToCartAsync(int cartId, ItemRecord item, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);
        const string sql = @$"
            INSERT INTO public.shopping_cart_items(cart_id, product_id, quantity)
            VALUES (@CartId, @ProductId, @Quantity)
            ON CONFLICT (cart_id, product_id)
            DO UPDATE SET quantity = shopping_cart_items.quantity + @Quantity;";

        var insertItem = new { CartId = cartId, item.Product.ProductId, item.Quantity };

        var result = await connection.ExecuteAsync(sql, insertItem);
        return result == 1;
    }

    public async Task<int?> CreateCartAsync(int userId, CartRecord cart, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            const string sql = @$"
            INSERT INTO public.shopping_cart(user_id)
	        VALUES (@userId)
            RETURNING cart_id;";

            var cartId = await connection.QueryFirstAsync<int>(sql, new { userId }, transaction);
            var result = await AddToCartAsync(connection, transaction, cartId, cart.Items, cancellationToken);

            if (result)
            {
                await transaction.CommitAsync();
                return cartId;
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

    private async Task<bool> AddToCartAsync(NpgsqlConnection connection, NpgsqlTransaction transaction, int cartId, JsonList<ItemRecord> items, CancellationToken cancellationToken)
    {
        const string sql = @$"
            INSERT INTO public.shopping_cart_items(cart_id, product_id, quantity)
            VALUES (@CartId, @ProductId, @Quantity)
            ON CONFLICT (cart_id, product_id)
            DO UPDATE SET quantity = shopping_cart_items.quantity + @Quantity;";

        var itemsToOrder = items.Select(x => new { CartId = cartId, x.Product.ProductId, x.Quantity });

        var result = await connection.ExecuteAsync(sql, itemsToOrder, transaction);
        return result >= 1;
    }

    public async Task<CartRecord> GetCartAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);

        const string sql = @$"
            SELECT
                s.cart_id AS {nameof(CartRecord.CartId)},
                s.user_id AS {nameof(CartRecord.UserId)},
                s.created_at AS {nameof(CartRecord.CreatedAt)},
                (SELECT json_agg(i) FROM (
                    SELECT
                        ci.item_id AS {nameof(ItemRecord.ItemId)},
                        ci.quantity AS {nameof(ItemRecord.Quantity)},
                        ci.created_at AS {nameof(ItemRecord.CreatedAt)},
                        (SELECT to_json(p) FROM (
                            SELECT
                                product_id AS {nameof(ProductRecord.ProductId)},
                                name AS {nameof(ProductRecord.Name)},
                                price AS {nameof(ProductRecord.Price)},
                                description AS {nameof(ProductRecord.Description)},
                                created_at AS {nameof(ProductRecord.CreatedAt)}
                            FROM public.products
                            WHERE product_id = ci.product_id) AS p) AS {nameof(ItemRecord.Product)}
                    FROM public.shopping_cart_items ci
                    WHERE ci.cart_id = s.cart_id
                ) AS i) AS {nameof(CartRecord.Items)}
	        FROM public.shopping_cart s
            WHERE s.user_id = @userId AND s.cart_id = @cartId;
        ";

        return await connection.QueryFirstOrDefaultAsync<CartRecord>(sql, new { userId, cartId });
    }

    public async Task RemoveCartAsync(int userId, int cartId, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);
        const string sql = @$"
            DELETE FROM shopping_cart
            WHERE user_id = @userId AND cart_id = @cart_id;";

        await connection.ExecuteAsync(sql, new { userId, cartId });
    }

    public async Task RemoveFromCartAsync(int userId, int cartId, int itemId, CancellationToken cancellationToken)
    {
        using var connection = await _database.OpenConnectionAsync(cancellationToken);
        const string sql = @$"
            DELETE FROM shopping_cart_items sci
            WHERE sci.item_id = @itemId
            AND sci.cart_id IN (SELECT cart_id FROM shopping_cart WHERE cart_id = @cartId and user_id = @userId);";

        await connection.ExecuteAsync(sql, new { userId, cartId, itemId });
    }
}