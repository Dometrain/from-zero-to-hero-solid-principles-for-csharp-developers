using Store.Application.Models;
using Store.Common.Results;

namespace Store.Application.Services;
public interface IOrderService
{
    Task<Result<Order>> GetOrderAsync(int userId, int orderId, CancellationToken cancellationToken);
    Task<Result<Paged<Order>>> GetOrdersAsync(int userId, int page, int pageSize, CancellationToken cancellationToken);
    Task<Result<Order>> CreateOrderAsync(int userId, int cartId, CancellationToken cancellationToken);
}