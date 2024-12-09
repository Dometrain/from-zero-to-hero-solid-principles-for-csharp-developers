using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public interface IReadOrders
{
    Task<OrderRecord> GetOrderAsync(int userId, int orderId, CancellationToken cancellationToken);
    Task<PagedRecord<OrderRecord>> GetOrdersAsync(int userId, int page, int pageSize, CancellationToken cancellationToken);
    Task<IEnumerable<OrderReportRecord>> GetOrderReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
}