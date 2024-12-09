using Store.Api.Contracts.Responses;
using Store.Application.Models;

namespace Store.Api.Mappers;

public static class ProductMapper
{
    public static PagedResponse<ProductResponse> Map(this Paged<Product> paged) => new PagedResponse<ProductResponse>
    {
        Page = paged.Page,
        TotalPages = paged.TotalPages,
        PageSize = paged.PageSize,
        Data = paged.Data.Select(x => Map(x)).ToList()
    };

    public static ProductResponse Map(this Product product) => new ProductResponse
    {
        ProductId = product.ProductId,
        Name = product.Name,
        Description = product.Description,
        Price = product.Price
    };
}