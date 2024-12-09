using Store.Application.Models;
using Store.Application.Mappers;
using Store.Infrastructure.Data;
using Store.Common.Results;

namespace Store.Application.Services;

public class ProductService : IProductService
{
    private readonly IReadProducts _productRepository;
    public ProductService(IReadProducts productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<Result<Product>> GetProductAsync(int productId, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetProductAsync(productId, cancellationToken);
        if (product == null)
            return new NotFoundResult<Product>();

        return new SuccessResult<Product>(product?.Map());
    }

    public async Task<Result<Paged<Product>>> GetProductsAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsAsync(page, pageSize, cancellationToken);
        return new SuccessResult<Paged<Product>>(products.Map());
    }
}