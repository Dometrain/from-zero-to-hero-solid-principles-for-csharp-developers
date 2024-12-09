using Store.Application.Models;
using Store.Common.Results;

namespace Store.Application.Services;
public interface ICartService
{
    Task<Result<Cart>> GetCartAsync(int userId, int cartId, CancellationToken cancellationToken);
    Task<Result<Cart>> CreateCartAsync(int userId, Cart cart, CancellationToken cancellationToken);
    Task<Result<Cart>> AddToCartAsync(int userId, int cartId, Item item, CancellationToken cancellationToken);
    Task RemoveFromCartAsync(int userId, int cartId, int itemId, CancellationToken cancellationToken);
    Task RemoveCartAsync(int userId, int cartId, CancellationToken cancellationToken);
}