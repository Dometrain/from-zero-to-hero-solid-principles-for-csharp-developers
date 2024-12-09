using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public interface IReadCart
{
    Task<CartRecord> GetCartAsync(int userId, int cartId, CancellationToken cancellationToken);
}