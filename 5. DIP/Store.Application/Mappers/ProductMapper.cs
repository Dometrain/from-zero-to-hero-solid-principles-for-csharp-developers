using Store.Application.Models;
using Store.Infrastructure.Models;

namespace Store.Application.Mappers;

public static class ProductMapper
{
    public static Product Map(this ProductRecord product) => new Product
    {
        ProductId = product.ProductId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CreatedAt = product.CreatedAt
    };

    public static ProductRecord Map(this Product product) => new ProductRecord
    {
        ProductId = product.ProductId,
        Name = product.Name,
        Price = product.Price,
        Description = product.Description,
        CreatedAt = product.CreatedAt
    };

    public static Paged<Product> Map(this PagedRecord<ProductRecord> products) => new Paged<Product>
    {
        Page = products.Page,
        PageSize = products.PageSize,
        TotalPages = products.TotalPages,
        Data = products.Data.Select(x => x.Map())
    };
}