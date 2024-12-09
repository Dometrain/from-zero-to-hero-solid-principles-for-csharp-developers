using Store.Infrastructure.Models;

namespace Store.Infrastructure.Data;

public interface IReadProducts
{
    Task<ProductRecord> GetProductAsync(int productId, CancellationToken cancellationToken);
    Task<PagedRecord<ProductRecord>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken);
}