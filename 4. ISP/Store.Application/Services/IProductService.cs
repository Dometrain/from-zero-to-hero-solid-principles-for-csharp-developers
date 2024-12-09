using Store.Application.Models;
using Store.Common.Results;

namespace Store.Application.Services;

public interface IProductService
{
    Task<Result<Product>> GetProductAsync(int productId, CancellationToken cancellationToken);
    Task<Result<Paged<Product>>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken);
}