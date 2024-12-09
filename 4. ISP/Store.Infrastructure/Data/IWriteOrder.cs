using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public interface IWriteOrder
{
    Task<int?> CreateOrderAsync(OrderRecord order, CancellationToken cancellationToken);
}