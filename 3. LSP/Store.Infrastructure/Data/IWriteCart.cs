using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public interface IWriteCart
{
    Task<bool> AddToCartAsync(int cartId, ItemRecord item, CancellationToken cancellationToken);
    Task<int?> CreateCartAsync(int userId, CartRecord cart, CancellationToken cancellationToken);
    Task RemoveCartAsync(int userId, int cartId, CancellationToken cancellationToken);
    Task RemoveFromCartAsync(int userId, int cartId, int itemId, CancellationToken cancellationToken);
}